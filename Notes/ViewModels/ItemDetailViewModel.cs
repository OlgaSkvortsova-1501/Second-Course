using Notes.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Notes.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private string itemId;
        private Item item;
        private string text;
        private string description;
        public ItemDetailViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            RemoveCommand = new Command(OnRemove);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }
        public string Id { get; set; }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public Command SaveCommand { get; }
        public Command RemoveCommand { get; }

        public async void LoadItemId(string itemId)
        {
            try
            {
                item = await DataStore.GetItemAsync(itemId);
                Id = item.Id;
                Text = item.Text;
                Description = item.Description;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(text)
                && !String.IsNullOrWhiteSpace(description);
        }

        private async void OnSave()
        {
            item.Text = Text;
            item.Description = Description;
            await DataStore.UpdateItemAsync(item);
            await Shell.Current.GoToAsync("..");
        }

        private async void OnRemove()
        {
            await DataStore.DeleteItemAsync(itemId);
            await Shell.Current.GoToAsync("..");
        }
    }
}
