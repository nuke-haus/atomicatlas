using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using Atlas.Core;
using Atlas.WorldGen;
using Atlas.Data;
using System.Collections;
using Codice.CM.SEIDInfo;
using System.Security.Principal;
using System.Reflection;
using System.Text.RegularExpressions;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Atlas.Logic
{
    public enum ErrorLogLevel
    {
        ERROR,
        WARNING,
        INFO
    }

    public class MainMenuManager : MonoBehaviour
    {
        public static MainMenuManager GlobalInstance
        {
            get;
            private set;
        }

        public event EventHandler OnClickRegenerateEvent;
        public event EventHandler OnClickExportEvent;
        public event EventHandler OnClickQuitEvent;

        [Header("Panel References")]
        [SerializeField]
        private GameObject uiRoot;

        [SerializeField]
        private GameObject settingsPanel;

        [SerializeField]
        private GameObject quitPanel;

        [SerializeField]
        private GameObject generatorSettingsPanel;

        [SerializeField]
        private GameObject gameSettingsPanel;

        [SerializeField]
        private GameObject errorLogPanel;

        [Header("Error Log")]
        [SerializeField]
        private GameObject errorLogListRoot;

        [SerializeField]
        private GameObject errorLogEntryPrefab;

        [Header("Generator Settings")]
        [SerializeField]
        private TMP_Dropdown strategyDropdown;

        [SerializeField]
        private TMP_Dropdown strategyConfigDropdown;

        [SerializeField]
        private Transform parameterListRoot;

        [SerializeField]
        private GameObject booleanEntryPrefab;

        [SerializeField]
        private GameObject numericEntryPrefab;

        [SerializeField]
        private GameObject stringEntryPrefab;

        [SerializeField]
        private GameObject numericRangeEntryPrefab;

        [SerializeField]
        private GameObject numericRangeEntryForGroupPrefab;

        [SerializeField]
        private GameObject numericRangeGroupPrefab;

        [Header("Gameplay Settings")]
        [SerializeField]
        private GameObject playerInfoEntryPrefab;

        [SerializeField]
        private RectTransform playerInfoListRoot;

        [SerializeField]
        private TMP_Dropdown playerCountDropdown;

        [SerializeField]
        private Toggle disciplesToggle;

        [SerializeField]
        private TMP_InputField proceduralNameChanceInput;

        private Dictionary<Type, Action<FieldInfo>> parameterEditorFunctions = new();
        private List<GameObject> parameterEditors = new();
        private List<PlayerInfoEntry> playerInfoEntries = new();
        private List<ErrorLogEntry> errorLogEntries = new();
        private StrategyConfigDefinition strategyConfigDefinition;
        private NumericRangeCollection currentNumericRangeGroup;
        private IDataManager dataManager;
        private ISettingsManager settingsManager;

        void Start()
        {
            GlobalInstance = this;
            Application.logMessageReceived += HandleExceptions;
            dataManager = DependencyInjector.Resolve<IDataManager>();
            settingsManager = DependencyInjector.Resolve<ISettingsManager>();

            InitializeDropdowns();
            InitializeParameterEditor();
        }

        void Update()
        {

        }

        private void InitializeParameterEditor()
        {
            parameterEditorFunctions[typeof(int)] = (field) => CreateIntEditor(field);
            parameterEditorFunctions[typeof(IntRange)] = (field) => CreateIntRangeEditor(field);
            parameterEditorFunctions[typeof(bool)] = (field) => CreateBoolEditor(field);

            // For MVP we don't need string editors
            // parameterEditorFunctions[typeof(string)] = (field) => CreateStringEditor(field);

            OnStrategyChanged();
            OnStrategyConfigChanged();
        }

        private void InitializeDropdowns()
        {
            var types = AtlasHelpers.GetTypesWithAttribute<StrategyAttribute>();
            var strategyList = new List<TMP_Dropdown.OptionData>();

            foreach (var strategyType in types)
            {
                var attribute = (StrategyAttribute)Attribute.GetCustomAttribute(strategyType, typeof(StrategyAttribute));
                var option = new TMP_Dropdown.OptionData(attribute.DisplayName.ToUpper());
                strategyList.Add(option);
            }

            strategyDropdown.options = strategyList;
            settingsManager.SetActiveStrategy(types.First());

            playerCountDropdown.value = 7; // 9 player option is default, it's at index 7
            proceduralNameChanceInput.text = settingsManager.ProceduralNameChance.ToString(); 
        }

        #region Generator Settings

        public void OnStrategyChanged()
        {
            var types = AtlasHelpers.GetTypesWithAttribute<StrategyAttribute>().ToList();
            var index = strategyDropdown.value;
            var selectedType = types[index];
            var attribute = (StrategyAttribute)Attribute.GetCustomAttribute(selectedType, typeof(StrategyAttribute));
            var data = dataManager.AllStrategyData.FirstOrDefault(data => data.GetType() == attribute.DataClassType);

            if (data == null || !data.StrategyConfigDefinitions.Any())
            {
                Debug.LogError("Cannot get data for strategy type " + selectedType.Name);
            }
            else
            {
                strategyConfigDefinition = data.StrategyConfigDefinitions.First();
                settingsManager.SetActiveStrategyConfigDefinition(strategyConfigDefinition);
                var options = data.StrategyConfigDefinitions.Select(config => new TMP_Dropdown.OptionData(config.Name.ToUpper())).ToList();
                strategyConfigDropdown.options = options;
            }
        }

        public void OnStrategyConfigChanged()
        {
            var types = AtlasHelpers.GetTypesWithAttribute<StrategyAttribute>().ToList();
            var index = strategyDropdown.value;
            var selectedType = types[index];
            var attribute = (StrategyAttribute)Attribute.GetCustomAttribute(selectedType, typeof(StrategyAttribute));
            var data = dataManager.AllStrategyData.FirstOrDefault(data => data.GetType() == attribute.DataClassType);

            var newValue = strategyConfigDropdown.value;
            strategyConfigDefinition = data.StrategyConfigDefinitions.ElementAt(newValue);

            UpdateConfigDefinitionEditor();
        }

        private void UpdateConfigDefinitionEditor()
        {
            // Delete all the old editors
            foreach (var gameObject in parameterEditors)
            {
                Destroy(gameObject);
            }
            parameterEditors.Clear();
            currentNumericRangeGroup = null;

            // Use reflection to grab all of the fields of the new config definition
            var type = strategyConfigDefinition.GetType();
            var currentGroupName = string.Empty;
            var numericRanges = new List<NumericRangeEntry>();

            var fields = type.GetFields().Where(f => f.IsPublic).ToList();
            fields.Sort((fieldA, fieldB) =>
            {
                if (fieldA.FieldType == typeof(string))
                {
                    return -1;
                }
                if (fieldB.FieldType == typeof(string))
                {
                    return 1;
                }
                return fieldA.FieldType.Name.CompareTo(fieldB.FieldType.Name);
            });

            foreach (var field in fields)
            {
                if (Attribute.IsDefined(field, typeof(IntRangeGroupAttribute)))
                {
                    var attr = field.GetCustomAttributes(typeof(IntRangeGroupAttribute), false);
                    var groupName = ((IntRangeGroupAttribute)attr[0]).GroupName;
                    var maxValue = ((IntRangeGroupAttribute)attr[0]).MaxValue;

                    if (groupName != currentGroupName)
                    {
                        currentGroupName = groupName;
                        currentNumericRangeGroup = Instantiate(numericRangeGroupPrefab, parameterListRoot).GetComponent<NumericRangeCollection>();
                        currentNumericRangeGroup.SetMaxValue(maxValue);
                        parameterEditors.Add(currentNumericRangeGroup.gameObject);
                    }
                }

                if (parameterEditorFunctions.ContainsKey(field.FieldType))
                {
                    parameterEditorFunctions[field.FieldType](field);
                }
                else
                {
                    if (field.FieldType != typeof(string))
                    {
                        Debug.LogError("Failed to create parameter editor for unsupported type: " + field.FieldType.Name);
                    }
                }
            }
        }

        private string SanitizeFieldName(string name)
        {
            var spacedText = Regex.Replace(name, "([a-z])([A-Z])", "$1 $2");
            return spacedText.ToUpper();
        }

        private void CreateIntRangeEditor(FieldInfo field)
        {
            var root = parameterListRoot;

            if (Attribute.IsDefined(field, typeof(IntRangeGroupAttribute)))
            {
                root = currentNumericRangeGroup.ContainerRoot;
            }

            NumericRangeEntry numericRangeEntry;

            if (root != parameterListRoot)
            {
                numericRangeEntry = Instantiate(numericRangeEntryForGroupPrefab, root).GetComponent<NumericRangeEntry>();
            }
            else
            {
                numericRangeEntry = Instantiate(numericRangeEntryPrefab, root).GetComponent<NumericRangeEntry>();
            }

            numericRangeEntry.SetLabel(SanitizeFieldName(field.Name));
            IntRange valueRange = (IntRange)field.GetValue(strategyConfigDefinition);
            numericRangeEntry.SetValues(valueRange.Min, valueRange.Max);
            numericRangeEntry.OnValueUpdate += () =>
            {
                field.SetValue(strategyConfigDefinition, numericRangeEntry.IntRangeValue);
            };

            if (root != parameterListRoot)
            {
                currentNumericRangeGroup.AddNumericRange(numericRangeEntry);
            }

            parameterEditors.Add(numericRangeEntry.gameObject);
        }

        private void CreateBoolEditor(FieldInfo field)
        {
            var boolEntry = Instantiate(booleanEntryPrefab, parameterListRoot).GetComponent<BooleanEntry>();
            boolEntry.SetLabel(SanitizeFieldName(field.Name));
            boolEntry.SetValue((bool)field.GetValue(strategyConfigDefinition));
            boolEntry.OnValueUpdate += () =>
            {
                field.SetValue(strategyConfigDefinition, boolEntry.Value);
            };

            parameterEditors.Add(boolEntry.gameObject);
        }

        private void CreateIntEditor(FieldInfo field)
        {
            var numericEntry = Instantiate(numericEntryPrefab, parameterListRoot).GetComponent<NumericEntry>();
            numericEntry.SetLabel(SanitizeFieldName(field.Name));
            numericEntry.SetValue((int)field.GetValue(strategyConfigDefinition));
            numericEntry.OnValueUpdate += () =>
            {
                field.SetValue(strategyConfigDefinition, numericEntry.Value);
            };

            parameterEditors.Add(numericEntry.gameObject);
        }

        private void CreateStringEditor(FieldInfo field)
        {
            var stringEntry = Instantiate(stringEntryPrefab, parameterListRoot).GetComponent<StringEntry>();
            stringEntry.SetLabel(SanitizeFieldName(field.Name));
            stringEntry.SetValue((string)field.GetValue(strategyConfigDefinition));
            stringEntry.OnValueUpdate += () =>
            {
                field.SetValue(strategyConfigDefinition, stringEntry.Value);
            };

            parameterEditors.Add(stringEntry.gameObject);
        }

        #endregion
        #region Gameplay Settings

        public void OnProceduralNameChanceChanged()
        {
            var text = proceduralNameChanceInput.text;
            if (text == string.Empty)
            {
                text = "0";
            }

            settingsManager.SetProceduralNameChance(Mathf.Clamp(int.Parse(text), 0, 100));
        }

        public void OnClickDisciplesToggle()
        {
            settingsManager.SetDisciples(disciplesToggle.isOn);

            for (int i = 0; i < playerInfoEntries.Count; i++)
            {
                var entry = playerInfoEntries[i];
                entry.SetShowTeam(disciplesToggle.isOn);
            }
        }

        public void OnPlayerCountChanged()
        {
            var count = playerCountDropdown.value + 2;

            if (count > playerInfoEntries.Count)
            {
                while (playerInfoEntries.Count < count)
                {
                    var playerInfoEntry = Instantiate(playerInfoEntryPrefab, playerInfoListRoot).GetComponent<PlayerInfoEntry>();
                    playerInfoEntries.Add(playerInfoEntry);
                }
            }
            else if (count < playerInfoEntries.Count)
            {
                while (playerInfoEntries.Count > count)
                {
                    var entry = playerInfoEntries[playerInfoEntries.Count - 1];
                    playerInfoEntries.Remove(entry);

                    Destroy(entry.gameObject);
                }
            }

            settingsManager.UpdatePlayerCount(count);

            var playerInfoList = settingsManager.AllPlayerInfo.ToList();
            for (int i = 0; i < count; i++)
            {
                var entry = playerInfoEntries[i];
                entry.SetPlayerInfo(playerInfoList[i]);
                entry.SetShowTeam(settingsManager.IsDisciples);
            }
        }

        #endregion
        #region Error Log

        public void AddErrorLogEntry(string text, ErrorLogLevel logLevel, bool showPanel)
        {
            var errorLogEntry = Instantiate(errorLogEntryPrefab).GetComponent<ErrorLogEntry>();
            errorLogEntry.SetText(text, logLevel.ToString());

            errorLogEntries.Add(errorLogEntry);

            if (showPanel)
            {
                settingsPanel.SetActive(true);
                SetSettingsPanelActive(errorLogPanel);
            }
        }

        public void ClearErrorLog()
        {
            foreach (var entry in errorLogEntries)
            {
                Destroy(entry.gameObject);
            }

            errorLogEntries.Clear();
        }

        private void HandleExceptions(string log, string stack, LogType type)
        {
            if (type == LogType.Exception && !settingsPanel.activeSelf)
            {
                // TODO: Format text properly for the error log entry. only supports two lines. 
                // errorLogText.text = log + "\n\n" + stack;
            }
        }

        #endregion

        public void SetUIActive(bool active)
        {
            uiRoot.SetActive(active);
        }

        public void OnClickRegenerate()
        {
            OnClickRegenerateEvent?.Invoke(this, new EventArgs());
        }

        public void OnClickQuit()
        {
            OnClickQuitEvent?.Invoke(this, new EventArgs());
        }

        public void OnClickToggleQuitPanel()
        {
            quitPanel.SetActive(!quitPanel.activeSelf);
        }

        public void OnClickToggleSettings()
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }

        public void OnClickExport()
        {
            OnClickExportEvent?.Invoke(this, new EventArgs());
        }

        public void SetSettingsPanelActive(GameObject panel)
        {
            gameSettingsPanel.SetActive(false);
            generatorSettingsPanel.SetActive(false);
            errorLogPanel.SetActive(false);
            panel.SetActive(true);
        }
    } 
}
