// This code has been written by AHMET ALP for the Unity Asset "AA Save and Load System".
// Link to the asset store page: https://u3d.as/2TxY
// Publisher contact: ahmetalp.business@gmail.com

using System;
using UnityEngine;
using UnityEngine.UI;

namespace AASave
{
    public class SampleSceneScript : MonoBehaviour
    {
        [Header("Save System")]
        [SerializeField][Tooltip("Save System component on the Save System GameObject.")] private SaveSystem saveSystem;

        [Space(10F)]

        [Header("Blocks")]
        [SerializeField] private Block[] blocks;

        [Space(10F)]

        [Header("Block Background")]
        [SerializeField] private UnityEngine.UI.Image blockBackground;

        [Space(10F)]

        [Header("Arrows")]
        [SerializeField] private GameObject leftArrow;
        [SerializeField] private GameObject rightArrow;

        [Space(10F)]

        [Header("Numerical Values")]
        [SerializeField][Tooltip("Time it takes to transition between blocks.")] private float transitionDuration = 0.5F;
        [SerializeField][Tooltip("Horizontal distance between each block.")] private float blockDistance = 1150F;
        [SerializeField][Tooltip("Movement speed of the blocks.")] private float movementSpeed = 0.5F;

        [Space(10F)]

        [Header("Output Texts")]
        [SerializeField] private Text warningText;
        [SerializeField] private Text successText;
        [SerializeField] private GameObject loadedText;
        [SerializeField] private GameObject arrayWarningText;
        [SerializeField] private GameObject resetedText;
        [SerializeField] private GameObject deletedText;

        [Space(10F)]

        [Header("Save Block")]
        [SerializeField] private InputField saveDataName;
        [SerializeField] private Dropdown saveDropdown;
        [SerializeField] private InputField saveInputString;
        [SerializeField] private GameObject saveBooleanArea;
        [SerializeField] private Toggle saveToggle;
        [SerializeField] private GameObject saveTrueText;
        [SerializeField] private GameObject saveFalseText;
        [SerializeField] private GameObject saveColorArea;
        [SerializeField] private InputField saveColorRedInput;
        [SerializeField] private InputField saveColorGreenInput;
        [SerializeField] private InputField saveColorBlueInput;
        [SerializeField] private InputField saveColorAlphaInput;
        [SerializeField] private Image saveColorExample;
        [SerializeField] private GameObject saveDateTimeArea;
        [SerializeField] private InputField saveYearArea;
        [SerializeField] private InputField saveMonthArea;
        [SerializeField] private InputField saveDayArea;
        [SerializeField] private GameObject saveQuaternionArea;
        [SerializeField] private InputField saveQuaternionX;
        [SerializeField] private InputField saveQuaternionY;
        [SerializeField] private InputField saveQuaternionZ;
        [SerializeField] private InputField saveQuaternionW;
        [SerializeField] private GameObject saveVector2Area;
        [SerializeField] private InputField saveVector2X;
        [SerializeField] private InputField saveVector2Y;
        [SerializeField] private GameObject saveVector2IntArea;
        [SerializeField] private InputField saveVector2IntX;
        [SerializeField] private InputField saveVector2IntY;
        [SerializeField] private GameObject saveVector3Area;
        [SerializeField] private InputField saveVector3X;
        [SerializeField] private InputField saveVector3Y;
        [SerializeField] private InputField saveVector3Z;
        [SerializeField] private GameObject saveVector3IntArea;
        [SerializeField] private InputField saveVector3IntX;
        [SerializeField] private InputField saveVector3IntY;
        [SerializeField] private InputField saveVector3IntZ;
        [SerializeField] private GameObject saveVector4Area;
        [SerializeField] private InputField saveVector4X;
        [SerializeField] private InputField saveVector4Y;
        [SerializeField] private InputField saveVector4Z;
        [SerializeField] private InputField saveVector4W;

        [Space(10F)]

        [Header("Load Block")]
        [SerializeField] private InputField loadDataName;
        [SerializeField] private GameObject loadDataTypeTitle;
        [SerializeField] private Dropdown loadDataTypeDropdown;
        [SerializeField] private InputField loadStringField;
        [SerializeField] private GameObject loadBooleanArea;
        [SerializeField] private Toggle loadToggle;
        [SerializeField] private GameObject laodTrueText;
        [SerializeField] private GameObject laodFalseText;
        [SerializeField] private GameObject loadColorArea;
        [SerializeField] private InputField loadRedField;
        [SerializeField] private InputField loadGreenField;
        [SerializeField] private InputField loadBlueField;
        [SerializeField] private InputField loadAlphaField;
        [SerializeField] private Image loadColorExample;
        [SerializeField] private GameObject loadDateTimeArea;
        [SerializeField] private InputField loadYear;
        [SerializeField] private InputField loadMonth;
        [SerializeField] private InputField loadDay;
        [SerializeField] private GameObject loadQuaternionArea;
        [SerializeField] private InputField loadQuaternionX;
        [SerializeField] private InputField loadQuaternionY;
        [SerializeField] private InputField loadQuaternionZ;
        [SerializeField] private InputField loadQuaternionW;
        [SerializeField] private GameObject loadVector2Area;
        [SerializeField] private InputField loadVector2X;
        [SerializeField] private InputField loadVector2Y;
        [SerializeField] private GameObject loadVector2IntArea;
        [SerializeField] private InputField loadVectorInt2X;
        [SerializeField] private InputField loadVector2IntY;
        [SerializeField] private GameObject loadVector3Area;
        [SerializeField] private InputField loadVector3X;
        [SerializeField] private InputField loadVector3Y;
        [SerializeField] private InputField loadVector3Z;
        [SerializeField] private GameObject loadVector3IntArea;
        [SerializeField] private InputField loadVector3IntX;
        [SerializeField] private InputField loadVector3IntY;
        [SerializeField] private InputField loadVector3IntZ;
        [SerializeField] private GameObject loadVector4Area;
        [SerializeField] private InputField loadVector4X;
        [SerializeField] private InputField loadVector4Y;
        [SerializeField] private InputField loadVector4Z;
        [SerializeField] private InputField loadVector4W;

        [Space(10F)]

        [Header("Delete Block")]
        [SerializeField] private InputField deleteDataName;

        [Tooltip("If the script is transitioning between two blocks, then this flag is set as true.")] private bool transitioning = false;
        [Tooltip("Index of the currently active block.")] private int currentBlock = 0;
        [Tooltip("Index of the target block while transitionting.")] private int targetBlock = 0;
        [Tooltip("This variable will be used while transitioning background colors.")] private float targetPoint = 0F;
        [Tooltip("Current direction where the blocks are moving.")] private MovementDirection currentDirection = MovementDirection.Left;
        [Tooltip("This will be used while moving the blocks.")] private float snapDistance;
        private float redInput, greenInput, blueInput, alphaInput;
        private int yearInput, monthInput, dayInput;

        private void Start()
        {
            InitializeBlocks();

            warningText.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (transitioning)
            {
                ChangeBackgroundColor();

                if (currentDirection == MovementDirection.Left)
                {
                    MoveBlocksToLeft();
                }
                else
                {
                    MoveBlocksToRight();
                }
            }
        }

        /// <summary>
        /// This method initializes the blocks.
        /// </summary>
        private void InitializeBlocks()
        {
            if (blocks.Length == 0)
            {
                this.enabled = false;
                return;
            }

            for (int i = 1; i < blocks.Length; i++)
            {
                blocks[i].blockObject.SetActive(false);
                blocks[i].rectTransform.anchoredPosition = new Vector3(blockDistance * i, 0F, 0F);
            }

            blocks[0].blockObject.SetActive(true);
            blocks[0].rectTransform.anchoredPosition = Vector3.zero;

            blockBackground.gameObject.SetActive(true);
            blockBackground.color = blocks[0].backgroundColor;

            leftArrow.SetActive(false);
            rightArrow.SetActive(true);

            saveInputString.gameObject.SetActive(true);
            saveInputString.contentType = InputField.ContentType.DecimalNumber;
        }

        /// <summary>
        /// This method is called when the user press the left arrow on the screen.
        /// </summary>
        public void MoveLeft()
        {
            if (!transitioning)
            {
                transitioning = true;

                targetBlock = currentBlock - 1;

                if (targetBlock == 0)
                {
                    leftArrow.SetActive(false);
                }
                else
                {
                    leftArrow.SetActive(true);
                }

                rightArrow.SetActive(true);

                blocks[targetBlock].blockObject.SetActive(true);
                currentDirection = MovementDirection.Left;

                warningText.gameObject.SetActive(false);

                DisableAllOutputs();
            }
        }

        /// <summary>
        /// This mehtod is called when the user press the right arrow on the screen.
        /// </summary>
        public void MoveRight()
        {
            if (!transitioning)
            {
                transitioning = true;

                targetBlock = currentBlock + 1;

                if (targetBlock == blocks.Length - 1)
                {
                    rightArrow.SetActive(false);
                }
                else
                {
                    rightArrow.SetActive(true);
                }

                leftArrow.SetActive(true);

                blocks[targetBlock].blockObject.SetActive(true);
                currentDirection = MovementDirection.Right;

                warningText.gameObject.SetActive(false);

                DisableAllOutputs();
            }
        }

        /// <summary>
        /// This method changes the background color of the block background.
        /// </summary>
        private void ChangeBackgroundColor()
        {
            targetPoint += Time.deltaTime / transitionDuration;

            blockBackground.color = Color.Lerp(blocks[currentBlock].backgroundColor, blocks[targetBlock].backgroundColor, targetPoint);

            if (targetPoint >= 1F)
            {
                // Color transition has been completed.

                targetPoint = 0F;

                blocks[currentBlock].blockObject.SetActive(false);
                blocks[targetBlock].blockObject.SetActive(true);

                if (blocks[currentBlock].blockName.Equals("Save Block"))
                {
                    ResetSaveArea();
                }
                else if (blocks[currentBlock].blockName.Equals("Load Block"))
                {
                    ResetLoadBlock();
                }
                else if (blocks[currentBlock].blockName.Equals("Delete Block"))
                {
                    ResetDeleteBlock();
                }

                currentBlock = targetBlock;

                transitioning = false;
            }
        }

        /// <summary>
        /// This method moves all the blocks to the left.
        /// </summary>
        private void MoveBlocksToLeft()
        {
            if (blocks[targetBlock].rectTransform.anchoredPosition.x == 0)
            {
                return;
            }
            else if (Mathf.Abs(blocks[targetBlock].rectTransform.anchoredPosition.x) < 40F)
            {
                snapDistance = blocks[targetBlock].rectTransform.anchoredPosition.x;

                for (int i = 0; i < blocks.Length; i++)
                {
                    blocks[i].rectTransform.anchoredPosition = new Vector3(blocks[i].rectTransform.anchoredPosition.x - snapDistance, 0F, 0F);
                }

                return;
            }

            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i].rectTransform.Translate(movementSpeed * Time.deltaTime * Vector3.right);
            }
        }

        /// <summary>
        /// This method moves all the blocks to the right.
        /// </summary>
        private void MoveBlocksToRight()
        {
            if (blocks[targetBlock].rectTransform.anchoredPosition.x == 0)
            {
                return;
            }
            else if (Mathf.Abs(blocks[targetBlock].rectTransform.anchoredPosition.x) < 40F)
            {
                snapDistance = blocks[targetBlock].rectTransform.anchoredPosition.x;

                for (int i = 0; i < blocks.Length; i++)
                {
                    blocks[i].rectTransform.anchoredPosition = new Vector3(blocks[i].rectTransform.anchoredPosition.x - snapDistance, 0F, 0F);
                }

                return;
            }

            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i].rectTransform.Translate(movementSpeed * Time.deltaTime * Vector3.left);
            }
        }

        /// <summary>
        /// This method is called when the data type dropdown in the save block is changed.
        /// </summary>
        public void ChangedSaveDataTypeDropdown()
        {
            DisableAllOutputs();

            warningText.gameObject.SetActive(false);
            warningText.fontSize = 45;
            successText.gameObject.SetActive(false);

            saveBooleanArea.SetActive(false);

            saveInputString.gameObject.SetActive(false);
            saveInputString.characterLimit = 0;
            saveInputString.text = "";
            saveInputString.placeholder.GetComponent<Text>().text = "Data Value";

            saveColorArea.SetActive(false);
            saveColorRedInput.text = "";
            saveColorGreenInput.text = "";
            saveColorBlueInput.text = "";
            saveColorAlphaInput.text = "";
            saveColorExample.color = new Color(0F, 0F, 0F, 1F);

            saveDateTimeArea.SetActive(false);
            saveYearArea.text = "";
            saveMonthArea.text = "";
            saveDayArea.text = "";

            saveQuaternionArea.SetActive(false);
            saveQuaternionX.text = "";
            saveQuaternionY.text = "";
            saveQuaternionZ.text = "";
            saveQuaternionW.text = "";

            saveVector2Area.SetActive(false);
            saveVector2X.text = "";
            saveVector2Y.text = "";

            saveVector2IntArea.SetActive(false);
            saveVector2IntX.text = "";
            saveVector2IntY.text = "";

            saveVector3Area.SetActive(false);
            saveVector3X.text = "";
            saveVector3Y.text = "";
            saveVector3Z.text = "";

            saveVector3IntArea.SetActive(false);
            saveVector3IntX.text = "";
            saveVector3IntY.text = "";
            saveVector3IntZ.text = "";

            saveVector4Area.SetActive(false);
            saveVector4X.text = "";
            saveVector4Y.text = "";
            saveVector4Z.text = "";
            saveVector4W.text = "";

            if (saveDropdown.options[saveDropdown.value].text.Equals("Boolean"))
            {
                saveBooleanArea.SetActive(true);
                return;
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Byte"))
            {
                saveInputString.gameObject.SetActive(true);
                saveInputString.contentType = InputField.ContentType.IntegerNumber;
                return;
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Char"))
            {
                saveInputString.gameObject.SetActive(true);
                saveInputString.contentType = InputField.ContentType.Standard;
                saveInputString.characterLimit = 1;
                return;
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Color"))
            {
                saveColorArea.SetActive(true);
                return;
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("DateTime"))
            {
                saveDateTimeArea.SetActive(true);
                return;
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Decimal"))
            {
                saveInputString.gameObject.SetActive(true);
                saveInputString.contentType = InputField.ContentType.DecimalNumber;
                return;
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Double"))
            {
                saveInputString.gameObject.SetActive(true);
                saveInputString.contentType = InputField.ContentType.DecimalNumber;
                return;
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Float"))
            {
                saveInputString.gameObject.SetActive(true);
                saveInputString.contentType = InputField.ContentType.DecimalNumber;
                return;
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Integer"))
            {
                saveInputString.gameObject.SetActive(true);
                saveInputString.contentType = InputField.ContentType.IntegerNumber;
                return;
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Long"))
            {
                saveInputString.gameObject.SetActive(true);
                saveInputString.contentType = InputField.ContentType.IntegerNumber;
                return;
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Quaternion"))
            {
                saveQuaternionArea.SetActive(true);
                return;
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Sbyte"))
            {
                saveInputString.gameObject.SetActive(true);
                saveInputString.contentType = InputField.ContentType.IntegerNumber;
                return;
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Short"))
            {
                saveInputString.gameObject.SetActive(true);
                saveInputString.contentType = InputField.ContentType.IntegerNumber;
                return;
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("String"))
            {
                saveInputString.gameObject.SetActive(true);
                saveInputString.contentType = InputField.ContentType.Standard;
                return;
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("TimeSpan"))
            {
                saveInputString.gameObject.SetActive(true);
                saveInputString.contentType = InputField.ContentType.IntegerNumber;
                saveInputString.placeholder.GetComponent<Text>().text = "Data Value (In Ticks)";
                return;
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Uint") || saveDropdown.options[saveDropdown.value].text.Equals("Ulong") || saveDropdown.options[saveDropdown.value].text.Equals("Ushort"))
            {
                saveInputString.gameObject.SetActive(true);
                saveInputString.contentType = InputField.ContentType.IntegerNumber;
                return;
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Vector 2"))
            {
                saveVector2Area.SetActive(true);
                return;
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Vector 2 Int"))
            {
                saveVector2IntArea.SetActive(true);
                return;
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Vector 3"))
            {
                saveVector3Area.SetActive(true);
                return;
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Vector 3 Int"))
            {
                saveVector3IntArea.SetActive(true);
                return;
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Vector 4"))
            {
                saveVector4Area.SetActive(true);
                return;
            }
        }

        /// <summary>
        /// This method is called when the boolean toggle on the save block is changed.
        /// </summary>
        public void ChangedSaveBooleanToogle()
        {
            DisableAllOutputs();

            if (saveToggle.isOn)
            {
                saveTrueText.SetActive(true);
                saveFalseText.SetActive(false);
            }
            else
            {
                saveTrueText.SetActive(false);
                saveFalseText.SetActive(true);
            }
        }

        /// <summary>
        /// This method is called when the value of the save system data value field is changed.
        /// </summary>
        public void ChangedSaveSystemStringField()
        {
            DisableAllOutputs();

            if (saveDropdown.options[saveDropdown.value].text.Equals("Byte"))
            {
                try
                {
                    if (Convert.ToInt32(saveInputString.text) < byte.MinValue)
                    {
                        saveInputString.text = byte.MinValue.ToString();
                    }
                    else if (Convert.ToInt32(saveInputString.text) > byte.MaxValue)
                    {
                        saveInputString.text = byte.MaxValue.ToString();
                    }
                }
                catch (FormatException)
                {
                    saveInputString.text = "";
                }
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Decimal"))
            {
                if (!string.IsNullOrEmpty(saveInputString.text) && !string.IsNullOrWhiteSpace(saveInputString.text))
                {
                    try
                    {
                        if (!saveInputString.text.Equals("-"))
                        {
                            Convert.ToDecimal(saveInputString.text);
                        }
                    }
                    catch (OverflowException)
                    {
                        if (saveInputString.text[0].ToString().Equals("-"))
                        {
                            saveInputString.text = decimal.MinValue.ToString();
                            warningText.text = "Decimal value cannot be smaller than " + decimal.MinValue + ".";
                            warningText.fontSize = 31;
                            warningText.gameObject.SetActive(true);
                            return;
                        }
                        else
                        {
                            saveInputString.text = decimal.MaxValue.ToString();
                            warningText.text = "Decimal value cannot be larger than " + decimal.MaxValue + ".";
                            warningText.fontSize = 31;
                            warningText.gameObject.SetActive(true);
                        }
                    }
                }
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Double"))
            {
                if (!string.IsNullOrEmpty(saveInputString.text) && !string.IsNullOrWhiteSpace(saveInputString.text))
                {
                    try
                    {
                        if (!saveInputString.text.Equals("-"))
                        {
                            Convert.ToDouble(saveInputString.text);
                        }
                    }
                    catch (OverflowException)
                    {
                        if (saveInputString.text[0].ToString().Equals("-"))
                        {
                            saveInputString.text = double.MinValue.ToString();
                            warningText.text = "Double value cannot be smaller than " + double.MinValue + ".";
                            warningText.fontSize = 31;
                            warningText.gameObject.SetActive(true);
                            return;
                        }
                        else
                        {
                            saveInputString.text = double.MaxValue.ToString();
                            warningText.text = "Double value cannot be larger than " + double.MaxValue + ".";
                            warningText.fontSize = 31;
                            warningText.gameObject.SetActive(true);
                        }
                    }
                }
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Float"))
            {
                if (!string.IsNullOrEmpty(saveInputString.text) && !string.IsNullOrWhiteSpace(saveInputString.text))
                {
                    try
                    {
                        if (!saveInputString.text.Equals("-"))
                        {
                            Convert.ToSingle(saveInputString.text);
                        }
                    }
                    catch (OverflowException)
                    {
                        if (saveInputString.text[0].ToString().Equals("-"))
                        {
                            saveInputString.text = float.MinValue.ToString();
                            warningText.text = "Float value cannot be smaller than " + float.MinValue + ".";
                            warningText.fontSize = 31;
                            warningText.gameObject.SetActive(true);
                            return;
                        }
                        else
                        {
                            saveInputString.text = float.MaxValue.ToString();
                            warningText.text = "Float value cannot be larger than " + float.MaxValue + ".";
                            warningText.fontSize = 31;
                            warningText.gameObject.SetActive(true);
                        }
                    }
                }
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Integer"))
            {
                if (!string.IsNullOrEmpty(saveInputString.text) && !string.IsNullOrWhiteSpace(saveInputString.text))
                {
                    try
                    {
                        if (!saveInputString.text.Equals("-"))
                        {
                            Convert.ToInt32(saveInputString.text);
                        }
                    }
                    catch (OverflowException)
                    {
                        if (!saveInputString.text[0].ToString().Equals("-"))
                        {
                            saveInputString.text = int.MaxValue.ToString();
                            warningText.text = "Integer value cannot be larger than " + int.MaxValue + ".";
                            warningText.gameObject.SetActive(true);
                        }
                        else
                        {
                            saveInputString.text = int.MinValue.ToString();
                            warningText.text = "Integer value cannot be smaller than " + int.MinValue + ".";
                            warningText.gameObject.SetActive(true);
                        }
                    }
                }
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Long"))
            {
                if (!string.IsNullOrEmpty(saveInputString.text) && !string.IsNullOrWhiteSpace(saveInputString.text))
                {
                    try
                    {
                        if (!saveInputString.text.Equals("-"))
                        {
                            Convert.ToInt64(saveInputString.text);
                        }
                    }
                    catch (OverflowException)
                    {
                        if (!saveInputString.text[0].ToString().Equals("-"))
                        {
                            saveInputString.text = long.MaxValue.ToString();
                            warningText.text = "Long value cannot be larger than " + long.MaxValue + ".";
                            warningText.gameObject.SetActive(true);
                            warningText.fontSize = 38;
                        }
                        else
                        {
                            saveInputString.text = long.MinValue.ToString();
                            warningText.text = "Long value cannot be smaller than " + long.MinValue + ".";
                            warningText.gameObject.SetActive(true);
                            warningText.fontSize = 38;
                        }
                    }
                }
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Sbyte"))
            {
                if (!string.IsNullOrEmpty(saveInputString.text) && !string.IsNullOrWhiteSpace(saveInputString.text))
                {
                    try
                    {
                        if (!saveInputString.text.Equals("-"))
                        {
                            Convert.ToSByte(saveInputString.text);
                        }
                    }
                    catch (OverflowException)
                    {
                        if (!saveInputString.text[0].ToString().Equals("-"))
                        {
                            saveInputString.text = sbyte.MaxValue.ToString();
                            warningText.text = "Sbyte value cannot be larger than " + sbyte.MaxValue + ".";
                            warningText.gameObject.SetActive(true);
                        }
                        else
                        {
                            saveInputString.text = sbyte.MinValue.ToString();
                            warningText.text = "Sbyte value cannot be smaller than " + sbyte.MinValue + ".";
                            warningText.gameObject.SetActive(true);
                        }
                    }
                }
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Short"))
            {
                if (!string.IsNullOrEmpty(saveInputString.text) && !string.IsNullOrWhiteSpace(saveInputString.text))
                {
                    try
                    {
                        if (!saveInputString.text.Equals("-"))
                        {
                            Convert.ToInt16(saveInputString.text);
                        }
                    }
                    catch (OverflowException)
                    {
                        if (!saveInputString.text[0].ToString().Equals("-"))
                        {
                            saveInputString.text = short.MaxValue.ToString();
                            warningText.text = "Short value cannot be larger than " + short.MaxValue + ".";
                            warningText.gameObject.SetActive(true);
                        }
                        else
                        {
                            saveInputString.text = short.MinValue.ToString();
                            warningText.text = "Short value cannot be smaller than " + short.MinValue + ".";
                            warningText.gameObject.SetActive(true);
                        }
                    }
                }
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("TimeSpan"))
            {
                if (!string.IsNullOrEmpty(saveInputString.text) && !string.IsNullOrWhiteSpace(saveInputString.text))
                {
                    try
                    {
                        if (!saveInputString.text.Equals("-"))
                        {
                            Convert.ToInt64(saveInputString.text);
                        }
                    }
                    catch (OverflowException)
                    {
                        if (!saveInputString.text[0].ToString().Equals("-"))
                        {
                            saveInputString.text = long.MaxValue.ToString();
                            warningText.text = "TimeSpan ticks value cannot be larger than " + long.MaxValue + ".";
                            warningText.gameObject.SetActive(true);
                            warningText.fontSize = 34;
                        }
                        else
                        {
                            saveInputString.text = long.MinValue.ToString();
                            warningText.text = "Timespan ticks value cannot be smaller than " + long.MinValue + ".";
                            warningText.gameObject.SetActive(true);
                            warningText.fontSize = 34;
                        }
                    }
                }
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Uint"))
            {
                if (!string.IsNullOrEmpty(saveInputString.text) && !string.IsNullOrWhiteSpace(saveInputString.text))
                {
                    try
                    {
                        if (!saveInputString.text.Equals("-"))
                        {
                            Convert.ToUInt32(saveInputString.text);
                        }
                    }
                    catch (OverflowException)
                    {
                        if (!saveInputString.text[0].ToString().Equals("-"))
                        {
                            saveInputString.text = uint.MaxValue.ToString();
                            warningText.text = "Uint value cannot be larger than " + uint.MaxValue + ".";
                            warningText.gameObject.SetActive(true);
                        }
                        else
                        {
                            saveInputString.text = uint.MinValue.ToString();
                            warningText.text = "Uint value cannot be smaller than " + uint.MinValue + ".";
                            warningText.gameObject.SetActive(true);
                        }
                    }
                }
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Ulong"))
            {
                if (!string.IsNullOrEmpty(saveInputString.text) && !string.IsNullOrWhiteSpace(saveInputString.text))
                {
                    try
                    {
                        if (!saveInputString.text.Equals("-"))
                        {
                            Convert.ToUInt64(saveInputString.text);
                        }
                    }
                    catch (OverflowException)
                    {
                        if (!saveInputString.text[0].ToString().Equals("-"))
                        {
                            saveInputString.text = ulong.MaxValue.ToString();
                            warningText.text = "Ulong value cannot be larger than " + ulong.MaxValue + ".";
                            warningText.gameObject.SetActive(true);
                            warningText.fontSize = 34;
                        }
                        else
                        {
                            saveInputString.text = ulong.MinValue.ToString();
                            warningText.text = "Ulong value cannot be smaller than " + ulong.MinValue + ".";
                            warningText.gameObject.SetActive(true);
                            warningText.fontSize = 34;
                        }
                    }
                }
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Ushort"))
            {
                if (!string.IsNullOrEmpty(saveInputString.text) && !string.IsNullOrWhiteSpace(saveInputString.text))
                {
                    try
                    {
                        if (!saveInputString.text.Equals("-"))
                        {
                            Convert.ToUInt16(saveInputString.text);
                        }
                    }
                    catch (OverflowException)
                    {
                        if (!saveInputString.text[0].ToString().Equals("-"))
                        {
                            saveInputString.text = ushort.MaxValue.ToString();
                            warningText.text = "Ushort value cannot be larger than " + ushort.MaxValue + ".";
                            warningText.gameObject.SetActive(true);
                            warningText.fontSize = 34;
                        }
                        else
                        {
                            saveInputString.text = ushort.MinValue.ToString();
                            warningText.text = "Ushort value cannot be smaller than " + ushort.MinValue + ".";
                            warningText.gameObject.SetActive(true);
                            warningText.fontSize = 34;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method is called when the player change the color field in the save block.
        /// </summary>
        public void ChangedSaveColors()
        {
            DisableAllOutputs();

            try
            {
                if (Convert.ToSingle(saveColorRedInput.text) < 0F)
                {
                    saveColorRedInput.text = "0";

                    warningText.text = "Red value must be between 0 and 1.";
                    warningText.gameObject.SetActive(true);
                }
                else if (Convert.ToSingle(saveColorRedInput.text) > 1F)
                {
                    saveColorRedInput.text = "1";

                    warningText.text = "Red value must be between 0 and 1.";
                    warningText.gameObject.SetActive(true);
                }
            }
            catch (FormatException)
            {
                if (!(saveColorRedInput.text.Contains(".") && !saveColorRedInput.text.StartsWith(".")))
                {
                    saveColorRedInput.text = "";
                }
            }

            if (String.IsNullOrEmpty(saveColorRedInput.text) || String.IsNullOrWhiteSpace(saveColorRedInput.text))
            {
                redInput = 0F;
            }
            else
            {
                redInput = Convert.ToSingle(saveColorRedInput.text);
            }

            try
            {
                if (Convert.ToSingle(saveColorGreenInput.text) < 0F)
                {
                    saveColorGreenInput.text = "0";

                    warningText.text = "Green value must be between 0 and 1.";
                    warningText.gameObject.SetActive(true);
                }
                else if (Convert.ToSingle(saveColorGreenInput.text) > 1F)
                {
                    saveColorGreenInput.text = "1";

                    warningText.text = "Green value must be between 0 and 1.";
                    warningText.gameObject.SetActive(true);
                }
            }
            catch (FormatException)
            {
                if (!(saveColorGreenInput.text.Contains(".") && !saveColorGreenInput.text.StartsWith(".")))
                {
                    saveColorGreenInput.text = "";
                }
            }

            if (String.IsNullOrEmpty(saveColorGreenInput.text) || String.IsNullOrWhiteSpace(saveColorGreenInput.text))
            {
                greenInput = 0F;
            }
            else
            {
                greenInput = Convert.ToSingle(saveColorGreenInput.text);
            }

            try
            {
                if (Convert.ToSingle(saveColorBlueInput.text) < 0F)
                {
                    saveColorBlueInput.text = "0";

                    warningText.text = "Blue value must be between 0 and 1.";
                    warningText.gameObject.SetActive(true);
                }
                else if (Convert.ToSingle(saveColorBlueInput.text) > 1F)
                {
                    saveColorBlueInput.text = "1";

                    warningText.text = "Blue value must be between 0 and 1.";
                    warningText.gameObject.SetActive(true);
                }
            }
            catch (FormatException)
            {
                if (!(saveColorBlueInput.text.Contains(".") && !saveColorBlueInput.text.StartsWith(".")))
                {
                    saveColorBlueInput.text = "";
                }
            }

            if (String.IsNullOrEmpty(saveColorBlueInput.text) || String.IsNullOrWhiteSpace(saveColorBlueInput.text))
            {
                blueInput = 0F;
            }
            else
            {
                blueInput = Convert.ToSingle(saveColorBlueInput.text);
            }

            try
            {
                if (Convert.ToSingle(saveColorAlphaInput.text) < 0F)
                {
                    saveColorAlphaInput.text = "0";

                    warningText.text = "Alpha value must be between 0 and 1.";
                    warningText.gameObject.SetActive(true);
                }
                else if (Convert.ToSingle(saveColorAlphaInput.text) > 1F)
                {
                    saveColorAlphaInput.text = "1";

                    warningText.text = "Alpha value must be between 0 and 1.";
                    warningText.gameObject.SetActive(true);
                }
            }
            catch (FormatException)
            {
                if (!(saveColorAlphaInput.text.Contains(".") && !saveColorAlphaInput.text.StartsWith(".")))
                {
                    saveColorAlphaInput.text = "";
                }
            }

            if (String.IsNullOrEmpty(saveColorAlphaInput.text) || String.IsNullOrWhiteSpace(saveColorAlphaInput.text))
            {
                alphaInput = 0F;
            }
            else
            {
                alphaInput = Convert.ToSingle(saveColorAlphaInput.text);
            }

            saveColorExample.color = new Color(redInput, greenInput, blueInput, 1F);
        }

        /// <summary>
        /// This method is called when the player change the DateTime field in the save block.
        /// </summary>
        public void ChangedSaveDateTime()
        {
            DisableAllOutputs();

            if (saveYearArea.text.Contains("-"))
            {
                saveYearArea.text = saveYearArea.text.Replace("-", "");
            }

            try
            {
                if (Convert.ToInt32(saveYearArea.text) > DateTime.MaxValue.Year)
                {
                    saveYearArea.text = DateTime.MaxValue.Year.ToString();

                    warningText.text = "Year value cannot be larger than " + DateTime.MaxValue.Year.ToString() + ".";
                    warningText.gameObject.SetActive(true);
                }
            }
            catch (FormatException)
            {
                saveYearArea.text = "";
            }

            if (String.IsNullOrEmpty(saveYearArea.text) || String.IsNullOrWhiteSpace(saveYearArea.text))
            {
                yearInput = 0;
            }
            else
            {
                yearInput = Convert.ToInt32(saveYearArea.text);
            }

            if (saveMonthArea.text.Contains("-"))
            {
                saveMonthArea.text = saveMonthArea.text.Replace("-", "");
            }

            try
            {
                if (Convert.ToInt32(saveMonthArea.text) > DateTime.MaxValue.Month)
                {
                    saveMonthArea.text = DateTime.MaxValue.Month.ToString();

                    warningText.text = "Month value cannot be larger than " + DateTime.MaxValue.Month.ToString() + ".";
                    warningText.gameObject.SetActive(true);
                }
            }
            catch (FormatException)
            {
                saveMonthArea.text = "";
            }

            if (String.IsNullOrEmpty(saveMonthArea.text) || String.IsNullOrWhiteSpace(saveMonthArea.text))
            {
                monthInput = 0;
            }
            else
            {
                monthInput = Convert.ToInt32(saveMonthArea.text);
            }

            if (saveDayArea.text.Contains("-"))
            {
                saveDayArea.text = saveDayArea.text.Replace("-", "");
            }

            try
            {
                if (Convert.ToInt32(saveDayArea.text) > DateTime.MaxValue.Day)
                {
                    saveDayArea.text = DateTime.MaxValue.Day.ToString();

                    warningText.text = "Day value cannot be larger than " + DateTime.MaxValue.Day.ToString() + ".";
                    warningText.gameObject.SetActive(true);
                }
            }
            catch (FormatException)
            {
                saveDayArea.text = "";
            }

            if (String.IsNullOrEmpty(saveDayArea.text) || String.IsNullOrWhiteSpace(saveDayArea.text))
            {
                dayInput = 0;
            }
            else
            {
                dayInput = Convert.ToInt32(saveDayArea.text);
            }
        }

        /// <summary>
        /// This method resets all the areas in the save block.
        /// </summary>
        private void ResetSaveArea()
        {
            for (int i = 0; i < saveDropdown.options.Count; i++)
            {
                if (saveDropdown.options[i].text.Equals("Float"))
                {
                    saveDropdown.value = i;
                    break;
                }
            }

            DisableAllOutputs();

            warningText.gameObject.SetActive(false);
            warningText.fontSize = 45;

            saveBooleanArea.SetActive(false);

            saveInputString.gameObject.SetActive(true);
            saveInputString.contentType = InputField.ContentType.DecimalNumber;
            saveInputString.characterLimit = 0;
            saveInputString.text = "";
            saveInputString.placeholder.GetComponent<Text>().text = "Data Value";

            saveColorArea.SetActive(false);
            saveColorRedInput.text = "";
            saveColorGreenInput.text = "";
            saveColorBlueInput.text = "";
            saveColorAlphaInput.text = "";
            saveColorExample.color = new Color(0F, 0F, 0F, 1F);

            saveDateTimeArea.SetActive(false);
            saveYearArea.text = "";
            saveMonthArea.text = "";
            saveDayArea.text = "";

            saveQuaternionArea.SetActive(false);
            saveQuaternionX.text = "";
            saveQuaternionY.text = "";
            saveQuaternionZ.text = "";
            saveQuaternionW.text = "";

            saveVector2Area.SetActive(false);
            saveVector2X.text = "";
            saveVector2Y.text = "";

            saveVector2IntArea.SetActive(false);
            saveVector2IntX.text = "";
            saveVector2IntY.text = "";

            saveVector3Area.SetActive(false);
            saveVector3X.text = "";
            saveVector3Y.text = "";
            saveVector3Z.text = "";

            saveVector3IntArea.SetActive(false);
            saveVector3IntX.text = "";
            saveVector3IntY.text = "";
            saveVector3IntZ.text = "";

            saveVector4Area.SetActive(false);
            saveVector4X.text = "";
            saveVector4Y.text = "";
            saveVector4Z.text = "";
            saveVector4W.text = "";
        }

        /// <summary>
        /// This method is called when the user press the save button on the save block.
        /// </summary>
        public void Save()
        {
            DisableAllOutputs();

            if (string.IsNullOrEmpty(saveDataName.text) || string.IsNullOrWhiteSpace(saveDataName.text))
            {
                warningText.text = "Data name cannot be blank.";
                warningText.fontSize = 45;
                warningText.gameObject.SetActive(true);
                return;
            }

            if (saveDropdown.options[saveDropdown.value].text.Equals("Boolean"))
            {
                saveSystem.Save(saveDataName.text ,saveToggle.isOn);
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Byte"))
            {
                if (string.IsNullOrEmpty(saveInputString.text) || string.IsNullOrWhiteSpace(saveInputString.text))
                {
                    saveInputString.text = "0";
                }

                saveSystem.Save(saveDataName.text, Convert.ToByte(saveInputString.text));
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Char"))
            {
                if (string.IsNullOrEmpty(saveInputString.text) || string.IsNullOrWhiteSpace(saveInputString.text))
                {
                    saveInputString.text = " ";
                }

                saveSystem.Save(saveDataName.text, saveInputString.text[0]);
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Color"))
            {
                if (string.IsNullOrEmpty(saveColorRedInput.text) || string.IsNullOrWhiteSpace(saveColorRedInput.text))
                {
                    saveColorRedInput.text = "0";
                }

                redInput = Convert.ToSingle(saveColorRedInput.text);

                if (string.IsNullOrEmpty(saveColorGreenInput.text) || string.IsNullOrWhiteSpace(saveColorGreenInput.text))
                {
                    saveColorGreenInput.text = "0";
                }

                greenInput = Convert.ToSingle(saveColorGreenInput.text);

                if (string.IsNullOrEmpty(saveColorBlueInput.text) || string.IsNullOrWhiteSpace(saveColorBlueInput.text))
                {
                    saveColorBlueInput.text = "0";
                }

                blueInput = Convert.ToSingle(saveColorBlueInput.text);

                if (string.IsNullOrEmpty(saveColorAlphaInput.text) || string.IsNullOrWhiteSpace(saveColorAlphaInput.text))
                {
                    saveColorAlphaInput.text = "0";
                }

                alphaInput = Convert.ToSingle(saveColorAlphaInput.text);

                saveSystem.Save(saveDataName.text, new Color(redInput, greenInput, blueInput, alphaInput));
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("DateTime"))
            {
                if (string.IsNullOrEmpty(saveYearArea.text) || string.IsNullOrWhiteSpace(saveYearArea.text))
                {
                    saveYearArea.text = DateTime.MinValue.Year.ToString();
                }

                yearInput = Convert.ToInt32(saveYearArea.text);

                if (string.IsNullOrEmpty(saveMonthArea.text) || string.IsNullOrWhiteSpace(saveMonthArea.text))
                {
                    saveMonthArea.text = DateTime.MinValue.Month.ToString();
                }

                monthInput = Convert.ToInt32(saveMonthArea.text);

                if (string.IsNullOrEmpty(saveDayArea.text) || string.IsNullOrWhiteSpace(saveDayArea.text))
                {
                    saveDayArea.text = DateTime.MinValue.Day.ToString();
                }

                dayInput = Convert.ToInt32(saveDayArea.text);

                saveSystem.Save(saveDataName.text, new DateTime(yearInput, monthInput, dayInput));
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Decimal"))
            {
                if (string.IsNullOrEmpty(saveInputString.text) || string.IsNullOrWhiteSpace(saveInputString.text))
                {
                    saveInputString.text = "0";
                }

                saveSystem.Save(saveDataName.text, Convert.ToDecimal(saveInputString.text));
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Double"))
            {
                if (string.IsNullOrEmpty(saveInputString.text) || string.IsNullOrWhiteSpace(saveInputString.text))
                {
                    saveInputString.text = "0";
                }

                saveSystem.Save(saveDataName.text, Convert.ToDouble(saveInputString.text));
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Float"))
            {
                if (string.IsNullOrEmpty(saveInputString.text) || string.IsNullOrWhiteSpace(saveInputString.text))
                {
                    saveInputString.text = "0";
                }

                saveSystem.Save(saveDataName.text, Convert.ToSingle(saveInputString.text));
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Integer"))
            {
                if (string.IsNullOrEmpty(saveInputString.text) || string.IsNullOrWhiteSpace(saveInputString.text))
                {
                    saveInputString.text = "0";
                }

                saveSystem.Save(saveDataName.text, Convert.ToInt32(saveInputString.text));
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Long"))
            {
                if (string.IsNullOrEmpty(saveInputString.text) || string.IsNullOrWhiteSpace(saveInputString.text))
                {
                    saveInputString.text = "0";
                }

                saveSystem.Save(saveDataName.text, Convert.ToInt64(saveInputString.text));
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Quaternion"))
            {
                if (string.IsNullOrEmpty(saveQuaternionX.text) || string.IsNullOrWhiteSpace(saveQuaternionX.text))
                {
                    saveQuaternionX.text = "0";
                }

                float quaternionXInput = Convert.ToSingle(saveQuaternionX.text);

                if (string.IsNullOrEmpty(saveQuaternionY.text) || string.IsNullOrWhiteSpace(saveQuaternionY.text))
                {
                    saveQuaternionY.text = "0";
                }

                float quaternionYInput = Convert.ToSingle(saveQuaternionY.text);

                if (string.IsNullOrEmpty(saveQuaternionZ.text) || string.IsNullOrWhiteSpace(saveQuaternionZ.text))
                {
                    saveQuaternionZ.text = "0";
                }

                float quaternionZInput = Convert.ToSingle(saveQuaternionZ.text);

                if (string.IsNullOrEmpty(saveQuaternionW.text) || string.IsNullOrWhiteSpace(saveQuaternionW.text))
                {
                    saveQuaternionW.text = "0";
                }

                float quaternionWInput = Convert.ToSingle(saveQuaternionW.text);

                saveSystem.Save(saveDataName.text, new Quaternion(quaternionXInput, quaternionYInput, quaternionZInput, quaternionWInput));
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Sbyte"))
            {
                if (string.IsNullOrEmpty(saveInputString.text) || string.IsNullOrWhiteSpace(saveInputString.text))
                {
                    saveInputString.text = "0";
                }

                saveSystem.Save(saveDataName.text, Convert.ToSByte(saveInputString.text));
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Short"))
            {
                if (string.IsNullOrEmpty(saveInputString.text) || string.IsNullOrWhiteSpace(saveInputString.text))
                {
                    saveInputString.text = "0";
                }

                saveSystem.Save(saveDataName.text, Convert.ToInt16(saveInputString.text));
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("String"))
            {
                saveSystem.Save(saveDataName.text, saveInputString.text);
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("TimeSpan"))
            {
                if (string.IsNullOrEmpty(saveInputString.text) || string.IsNullOrWhiteSpace(saveInputString.text))
                {
                    saveInputString.text = "0";
                }

                saveSystem.Save(saveDataName.text, new TimeSpan(Convert.ToInt64(saveInputString.text)));
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Uint"))
            {
                if (string.IsNullOrEmpty(saveInputString.text) || string.IsNullOrWhiteSpace(saveInputString.text))
                {
                    saveInputString.text = "0";
                }

                saveSystem.Save(saveDataName.text, Convert.ToUInt32(saveInputString.text));
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Ulong"))
            {
                if (string.IsNullOrEmpty(saveInputString.text) || string.IsNullOrWhiteSpace(saveInputString.text))
                {
                    saveInputString.text = "0";
                }

                saveSystem.Save(saveDataName.text, Convert.ToUInt64(saveInputString.text));
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Ushort"))
            {
                if (string.IsNullOrEmpty(saveInputString.text) || string.IsNullOrWhiteSpace(saveInputString.text))
                {
                    saveInputString.text = "0";
                }

                saveSystem.Save(saveDataName.text, Convert.ToUInt16(saveInputString.text));
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Vector 2"))
            {
                if (string.IsNullOrEmpty(saveVector2X.text) || string.IsNullOrWhiteSpace(saveVector2X.text))
                {
                    saveVector2X.text = "0";
                }

                float inputX = Convert.ToSingle(saveVector2X.text);

                if (string.IsNullOrEmpty(saveVector2Y.text) || string.IsNullOrWhiteSpace(saveVector2Y.text))
                {
                    saveVector2Y.text = "0";
                }

                float inputY = Convert.ToSingle(saveVector2Y.text);

                saveSystem.Save(saveDataName.text, new Vector2(inputX, inputY));
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Vector 2 Int"))
            {
                if (string.IsNullOrEmpty(saveVector2IntX.text) || string.IsNullOrWhiteSpace(saveVector2IntX.text))
                {
                    saveVector2IntX.text = "0";
                }

                int inputIntX = Convert.ToInt32(saveVector2IntX.text);

                if (string.IsNullOrEmpty(saveVector2IntY.text) || string.IsNullOrWhiteSpace(saveVector2IntY.text))
                {
                    saveVector2IntY.text = "0";
                }

                int inputIntY = Convert.ToInt32(saveVector2IntY.text);

                saveSystem.Save(saveDataName.text, new Vector2Int(inputIntX, inputIntY));
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Vector 3"))
            {
                if (string.IsNullOrEmpty(saveVector3X.text) || string.IsNullOrWhiteSpace(saveVector3X.text))
                {
                    saveVector3X.text = "0";
                }

                float inputX = Convert.ToSingle(saveVector3X.text);

                if (string.IsNullOrEmpty(saveVector3Y.text) || string.IsNullOrWhiteSpace(saveVector3Y.text))
                {
                    saveVector3Y.text = "0";
                }

                float inputY = Convert.ToSingle(saveVector3Y.text);

                if (string.IsNullOrEmpty(saveVector3Z.text) || string.IsNullOrWhiteSpace(saveVector3Z.text))
                {
                    saveVector3Z.text = "0";
                }

                float inputZ = Convert.ToSingle(saveVector3Z.text);

                saveSystem.Save(saveDataName.text, new Vector3(inputX, inputY, inputZ));
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Vector 3 Int"))
            {
                if (string.IsNullOrEmpty(saveVector3IntX.text) || string.IsNullOrWhiteSpace(saveVector3IntX.text))
                {
                    saveVector3IntX.text = "0";
                }

                int inputX = Convert.ToInt32(saveVector3IntX.text);

                if (string.IsNullOrEmpty(saveVector3IntY.text) || string.IsNullOrWhiteSpace(saveVector3IntY.text))
                {
                    saveVector3IntY.text = "0";
                }

                int inputY = Convert.ToInt32(saveVector3IntY.text);

                if (string.IsNullOrEmpty(saveVector3IntZ.text) || string.IsNullOrWhiteSpace(saveVector3IntZ.text))
                {
                    saveVector3IntZ.text = "0";
                }

                int inputZ = Convert.ToInt32(saveVector3IntZ.text);

                saveSystem.Save(saveDataName.text, new Vector3Int(inputX, inputY, inputZ));
            }
            else if (saveDropdown.options[saveDropdown.value].text.Equals("Vector 4"))
            {
                if (string.IsNullOrEmpty(saveVector4X.text) || string.IsNullOrWhiteSpace(saveVector4X.text))
                {
                    saveVector4X.text = "0";
                }

                float inputX = Convert.ToSingle(saveVector4X.text);

                if (string.IsNullOrEmpty(saveVector4Y.text) || string.IsNullOrWhiteSpace(saveVector4Y.text))
                {
                    saveVector4Y.text = "0";
                }

                float inputY = Convert.ToSingle(saveVector4Y.text);

                if (string.IsNullOrEmpty(saveVector4Z.text) || string.IsNullOrWhiteSpace(saveVector4Z.text))
                {
                    saveVector4Z.text = "0";
                }

                float inputZ = Convert.ToSingle(saveVector4Z.text);

                if (string.IsNullOrEmpty(saveVector4W.text) || string.IsNullOrWhiteSpace(saveVector4W.text))
                {
                    saveVector4W.text = "0";
                }

                float inputW = Convert.ToSingle(saveVector4W.text);

                saveSystem.Save(saveDataName.text, new Vector4(inputX, inputY, inputZ, inputW));
            }

            EnableSuccessFeedback("Saved!");
        }

        /// <summary>
        /// This mehtod enables the success feedback text.
        /// </summary>
        private void EnableSuccessFeedback(string feedbackText)
        {
            successText.text = feedbackText;
            successText.gameObject.SetActive(true);
        }

        /// <summary>
        /// This method disables all the output texts such as warnings and errors.
        /// </summary>
        public void DisableAllOutputs()
        {
            warningText.gameObject.SetActive(false);
            successText.gameObject.SetActive(false);
            loadedText.SetActive(false);
            arrayWarningText.SetActive(false);
            resetedText.SetActive(false);
            deletedText.SetActive(false);
        }

        /// <summary>
        /// This method is called when the user press the load button.
        /// </summary>
        public void Load()
        {
            DisableAllOutputs();

            if (string.IsNullOrEmpty(loadDataName.text) || string.IsNullOrWhiteSpace(loadDataName.text))
            {
                warningText.text = "Data name cannot be blank.";
                warningText.fontSize = 45;
                warningText.gameObject.SetActive(true);
                return;
            }

            if (!saveSystem.DoesDataExists(loadDataName.text))
            {
                warningText.text = "Data does not exist.";
                warningText.fontSize = 45;
                warningText.gameObject.SetActive(true);
                return;
            }

            ClearLoadBlock();

            AASave.DataTypes dataType = saveSystem.GetDataType(loadDataName.text);
            loadDataTypeTitle.SetActive(true);
            loadDataTypeDropdown.gameObject.SetActive(true);

            switch (dataType)
            {
                case DataTypes.Bool:
                    loadDataTypeDropdown.value = 0;
                    bool readValueBool = saveSystem.Load(loadDataName.text).AsBool();
                    loadToggle.isOn = readValueBool;
                    laodTrueText.SetActive(readValueBool);
                    laodFalseText.SetActive(!readValueBool);
                    loadBooleanArea.SetActive(true);

                    break;
                case DataTypes.Byte:
                    loadDataTypeDropdown.value = 1;
                    byte readValueByte = saveSystem.Load(loadDataName.text).AsByte();
                    loadStringField.gameObject.SetActive(true);
                    loadStringField.text = readValueByte.ToString();

                    break;
                case DataTypes.Char:
                    loadDataTypeDropdown.value = 2;
                    char readValueChar = saveSystem.Load(loadDataName.text).AsChar();
                    loadStringField.gameObject.SetActive(true);
                    loadStringField.text = readValueChar.ToString();

                    break;
                case DataTypes.Color:
                    loadDataTypeDropdown.value = 3;
                    Color readValueColor = saveSystem.Load(loadDataName.text).AsColor();
                    loadRedField.text = readValueColor.r.ToString();
                    loadGreenField.text = readValueColor.g.ToString();
                    loadBlueField.text = readValueColor.b.ToString();
                    loadAlphaField.text = readValueColor.a.ToString();
                    loadColorExample.color = new Color(readValueColor.r, readValueColor.g, readValueColor.b, 1F);
                    loadColorArea.SetActive(true);

                    break;
                case DataTypes.DateTime:
                    loadDataTypeDropdown.value = 4;
                    DateTime readValueDateTime = saveSystem.Load(loadDataName.text).AsDateTime();
                    loadYear.text = readValueDateTime.Year.ToString();
                    loadMonth.text = readValueDateTime.Month.ToString();
                    loadDay.text = readValueDateTime.Day.ToString();
                    loadDateTimeArea.SetActive(true);

                    break;
                case DataTypes.Decimal:
                    loadDataTypeDropdown.value = 5;
                    decimal readValueDecimal = saveSystem.Load(loadDataName.text).AsDecimal();
                    loadStringField.gameObject.SetActive(true);
                    loadStringField.text = readValueDecimal.ToString();

                    break;
                case DataTypes.Double:
                    loadDataTypeDropdown.value = 6;
                    double readValueDouble = saveSystem.Load(loadDataName.text).AsDouble();
                    loadStringField.gameObject.SetActive(true);
                    loadStringField.text = readValueDouble.ToString();

                    break;
                case DataTypes.Float:
                    loadDataTypeDropdown.value = 7;
                    float readValueFloat = saveSystem.Load(loadDataName.text).AsFloat();
                    loadStringField.gameObject.SetActive(true);
                    loadStringField.text = readValueFloat.ToString();

                    break;
                case DataTypes.Int:
                    loadDataTypeDropdown.value = 8;
                    float readValueInt = saveSystem.Load(loadDataName.text).AsInt();
                    loadStringField.gameObject.SetActive(true);
                    loadStringField.text = readValueInt.ToString();

                    break;
                case DataTypes.Long:
                    loadDataTypeDropdown.value = 9;
                    long readValueLong = saveSystem.Load(loadDataName.text).AsLong();
                    loadStringField.gameObject.SetActive(true);
                    loadStringField.text = readValueLong.ToString();

                    break;
                case DataTypes.Quaternion:
                    loadDataTypeDropdown.value = 10;
                    Quaternion readValueQuaternion = saveSystem.Load(loadDataName.text).AsQuaternion();
                    loadQuaternionX.text = readValueQuaternion.x.ToString();
                    loadQuaternionY.text = readValueQuaternion.y.ToString();
                    loadQuaternionZ.text = readValueQuaternion.z.ToString();
                    loadQuaternionW.text = readValueQuaternion.w.ToString();
                    loadQuaternionArea.SetActive(true);

                    break;
                case DataTypes.Sbyte:
                    loadDataTypeDropdown.value = 11;
                    sbyte readValueSbyte = saveSystem.Load(loadDataName.text).AsSbyte();
                    loadStringField.gameObject.SetActive(true);
                    loadStringField.text = readValueSbyte.ToString();

                    break;
                case DataTypes.Short:
                    loadDataTypeDropdown.value = 12;
                    short readValueShort = saveSystem.Load(loadDataName.text).AsShort();
                    loadStringField.gameObject.SetActive(true);
                    loadStringField.text = readValueShort.ToString();

                    break;
                case DataTypes.String:
                    loadDataTypeDropdown.value = 13;
                    string readValueString = saveSystem.Load(loadDataName.text).AsString();
                    loadStringField.gameObject.SetActive(true);
                    loadStringField.text = readValueString;

                    break;
                case DataTypes.TimeSpan:
                    loadDataTypeDropdown.value = 14;
                    TimeSpan readValueTimeSpan = saveSystem.Load(loadDataName.text).AsTimeSpan();
                    loadStringField.gameObject.SetActive(true);
                    loadStringField.text = readValueTimeSpan.Ticks.ToString();

                    break;
                case DataTypes.Uint:
                    loadDataTypeDropdown.value = 15;
                    uint readValueUint = saveSystem.Load(loadDataName.text).AsUint();
                    loadStringField.gameObject.SetActive(true);
                    loadStringField.text = readValueUint.ToString();

                    break;
                case DataTypes.Ulong:
                    loadDataTypeDropdown.value = 16;
                    ulong readValueUlong = saveSystem.Load(loadDataName.text).AsUlong();
                    loadStringField.gameObject.SetActive(true);
                    loadStringField.text = readValueUlong.ToString();

                    break;
                case DataTypes.Ushort:
                    loadDataTypeDropdown.value = 17;
                    ushort readValueUshort = saveSystem.Load(loadDataName.text).AsUshort();
                    loadStringField.gameObject.SetActive(true);
                    loadStringField.text = readValueUshort.ToString();

                    break;
                case DataTypes.Vector2:
                    loadDataTypeDropdown.value = 18;
                    Vector2 readValueVector2 = saveSystem.Load(loadDataName.text).AsVector2();
                    loadVector2X.text = readValueVector2.x.ToString();
                    loadVector2Y.text = readValueVector2.y.ToString();
                    loadVector2Area.SetActive(true);

                    break;
                case DataTypes.Vector2Int:
                    loadDataTypeDropdown.value = 19;
                    Vector2Int readValueVector2Int = saveSystem.Load(loadDataName.text).AsVector2Int();
                    loadVectorInt2X.text = readValueVector2Int.x.ToString();
                    loadVector2IntY.text = readValueVector2Int.y.ToString();
                    loadVector2IntArea.SetActive(true);

                    break;
                case DataTypes.Vector3:
                    loadDataTypeDropdown.value = 20;
                    Vector3 readValueVector3 = saveSystem.Load(loadDataName.text).AsVector3();
                    loadVector3X.text = readValueVector3.x.ToString();
                    loadVector3Y.text = readValueVector3.y.ToString();
                    loadVector3Z.text = readValueVector3.z.ToString();
                    loadVector3Area.SetActive(true);

                    break;
                case DataTypes.Vector3Int:
                    loadDataTypeDropdown.value = 21;
                    Vector3Int readValueVector3Int = saveSystem.Load(loadDataName.text).AsVector3Int();
                    loadVector3IntX.text = readValueVector3Int.x.ToString();
                    loadVector3IntY.text = readValueVector3Int.y.ToString();
                    loadVector3IntZ.text = readValueVector3Int.z.ToString();
                    loadVector3IntArea.SetActive(true);

                    break;
                case DataTypes.Vector4:
                    loadDataTypeDropdown.value = 22;
                    Vector4 readValueVector4 = saveSystem.Load(loadDataName.text).AsVector4();
                    loadVector4X.text = readValueVector4.x.ToString();
                    loadVector4Y.text = readValueVector4.y.ToString();
                    loadVector4Z.text = readValueVector4.z.ToString();
                    loadVector4W.text = readValueVector4.w.ToString();
                    loadVector4Area.SetActive(true);

                    break;
                default:
                    loadDataTypeTitle.SetActive(false);
                    loadDataTypeDropdown.gameObject.SetActive(false);

                    loadedText.SetActive(false);
                    arrayWarningText.SetActive(true);

                    break;
            }
        }

        /// <summary>
        /// This method resets the entire load block.
        /// </summary>
        public void ResetLoadBlock()
        {
            loadDataName.text = "";

            loadDataTypeTitle.SetActive(false);
            loadDataTypeDropdown.gameObject.SetActive(false);

            loadStringField.gameObject.SetActive(false);

            loadBooleanArea.SetActive(false);
            loadColorArea.SetActive(false);
            loadDateTimeArea.SetActive(false);
            loadQuaternionArea.SetActive(false);
            loadVector2Area.SetActive(false);
            loadVector2IntArea.SetActive(false);
            loadVector3Area.SetActive(false);
            loadVector3IntArea.SetActive(false);
            loadVector4Area.SetActive(false);
        }

        /// <summary>
        /// This method resets the entire Delete block.
        /// </summary>
        public void ResetDeleteBlock()
        {
            deleteDataName.text = "";
        }

        /// <summary>
        /// This method clears the load block before loading a game data.
        /// </summary>
        public void ClearLoadBlock()
        {
            loadDataTypeTitle.SetActive(false);
            loadDataTypeDropdown.gameObject.SetActive(false);

            loadStringField.gameObject.SetActive(false);

            loadBooleanArea.SetActive(false);
            loadColorArea.SetActive(false);
            loadDateTimeArea.SetActive(false);
            loadQuaternionArea.SetActive(false);
            loadVector2Area.SetActive(false);
            loadVector2IntArea.SetActive(false);
            loadVector3Area.SetActive(false);
            loadVector3IntArea.SetActive(false);
            loadVector4Area.SetActive(false);
        }

        /// <summary>
        /// This method is called when the user press the Delete button in the Delete Block.
        /// </summary>
        public void DeleteData()
        {
            DisableAllOutputs();

            if (string.IsNullOrEmpty(deleteDataName.text) || string.IsNullOrWhiteSpace(deleteDataName.text))
            {
                warningText.text = "Data name cannot be blank.";
                warningText.fontSize = 45;
                warningText.gameObject.SetActive(true);
                return;
            }

            if (!saveSystem.DoesDataExists(deleteDataName.text))
            {
                warningText.text = "Data does not exist.";
                warningText.fontSize = 45;
                warningText.gameObject.SetActive(true);
                return;
            }

            saveSystem.Delete(deleteDataName.text);
            deletedText.SetActive(true);
        }


        [System.Serializable]
        public struct Block
        {
            public string blockName;
            public GameObject blockObject;
            public RectTransform rectTransform;
            public Color backgroundColor;
        };

        private enum MovementDirection
        {
            Left,
            Right
        }
    }
}
