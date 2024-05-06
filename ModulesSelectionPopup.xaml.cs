
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using System.Drawing;
using System.Reflection;
using Syncfusion.Maui.Core;

namespace MauiController;

public partial class ModulesSelectionPopup : Popup{

    private MainPage ParentForm;
    private string SelectedModuleType = "";
    List<Button> moduleTypesButtons = new List<Button>();
    List<string> moduleTypesString = new List<string>();
    Type[] possibleModules = { };
    Button lastSelectedButton = null;

    public ModulesSelectionPopup()
	{
		InitializeComponent();
	}

    public void setParentForm(MainPage _parentForm)
    {
        ParentForm = _parentForm;
        CreateButtonForEachModuleClass();
        cbChannel.ItemsSource = ParentForm.midiChannels;
        cbChannel.SelectedIndex = 0;
    }

    private void CreateButtonForEachModuleClass()
    {
        possibleModules = ParentForm.GetAllPossibleModules();
        int i = 100;
        double Height = 250;
        double ButtonHeight = 20;

        foreach (Type possibleModule in possibleModules)
        {
            Height = Height + PopupVerticalStack.Spacing + ButtonHeight*2;
            ModulesSelectionPopupPage.Size = new(500, Height);

            string className = possibleModule.FullName;
            string constantName = "moduleType";
            Type classType = Type.GetType(className);
            FieldInfo constantField = classType.GetField(constantName);
            object moduleType = constantField.GetValue(null);

            moduleTypesButtons.Add(new Button());
            moduleTypesString.Add((string)possibleModule.Name);

            moduleTypesButtons[i - 100].BorderWidth = 0;
            moduleTypesButtons[i - 100].StyleId = possibleModule.Name;
            moduleTypesButtons[i - 100].WidthRequest = 300;
            moduleTypesButtons[i - 100].HeightRequest = ButtonHeight;
            moduleTypesButtons[i - 100].ZIndex = i;
            moduleTypesButtons[i - 100].Text = (string)moduleType;
            moduleTypesButtons[i - 100].Clicked += new System.EventHandler(this.ChangeSelectedModule);

            PopupVerticalStack.Add(moduleTypesButtons[i - 100]);
            i++;
        }
    }

    private void ChangeSelectedModule(object sender, EventArgs e)
    {
        string buttonName = "";
        int reccomendedIndex = 0;

        lastSelectedButton = (Button)sender;

        foreach (Button b in moduleTypesButtons)
        {
            
            b.BorderColor = ModuleEntryName.TextColor;
            if (b == lastSelectedButton)
            {
                b.BorderWidth = 2;
            }
            else
            {
                b.BorderWidth = 0;
            }
        }

        SelectedModuleType = lastSelectedButton.Text;
        buttonName = lastSelectedButton.Text;

        if (buttonName.Contains(" by"))
        {
            int trimIndex = buttonName.IndexOf(" by");

            if (trimIndex > 0)
            {
                buttonName = buttonName.Substring(0, trimIndex);
            }
        }

        reccomendedIndex = ParentForm.ReturnNumOfSelectedTypeModules(GetSelectedModuleType(lastSelectedButton));
        reccomendedIndex++;

        if (reccomendedIndex > 1)
        {
            buttonName = $"{buttonName}_{reccomendedIndex}";
        }

        ModuleEntryName.Text = buttonName;
    }

    private string GetSelectedModuleType(Button button)
    {
        int buttonIndex = button.ZIndex;
        string moduleType = moduleTypesString[buttonIndex - 100];

        return moduleType;
    }

    private void bAddThisModule_Click(object sender, EventArgs e)
    {

        if (SelectedModuleType == "")
        {
            
            //DisplayAlert("Alert", "You have been alerted", "OK");
            //MessageBox.Show("Select a module type to add to your pedalboard");
        }
        else if (ModuleEntryName.Text == "")
        {
            //MessageBox.Show("Insert a name for the module you want to add");
        }
        else if (!ParentForm.IsTheModuleNameFree(ModuleEntryName.Text))
        {
            //MessageBox.Show("The typed module name is already in use in this pedalboard!");
        }
        else
        {
            ///il tuo problema consiste nel dover richiamare, dall'interno del Form2, un metodo non statico (diciamo FunzioneF1) che appartiene al Form1.
            ///1) dichiara all'interno di Form2 una property Public e WriteOnly dello stesso tipo di Form1 (chiamiamola ParentForm) con relativa variabile privata (chiamiamola m_ParentF)
            ///2) in Form1 subito prima di richiamare Form2 scrivi: Form2.ParentForm = Me
            ///3) dopodiche' in Form2 dovresti tranquillamente poter scrivere:
            ParentForm.AddModule(lastSelectedButton, GetSelectedModuleType(lastSelectedButton), Convert.ToInt16(cbChannel.SelectedItem), ModuleEntryName.Text);
            this.Close();
        }
    }
}