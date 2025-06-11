using System.Collections.ObjectModel;
using final_Samira_Kilic.Model;

namespace final_Samira_Kilic;

public partial class Yapilacaklar : ContentPage
{
    private ObservableCollection<TodoTask> tasks = new();
    private TodoTask editingTask = null;
    private Notlar notlarService = new();

    public Yapilacaklar()
    {
        InitializeComponent();
        taskCollectionView.ItemsSource = tasks;
        LoadTasks();
    }

    private async void LoadTasks()
    {
        var loaded = await notlarService.GetTasks();
        tasks.Clear();
        foreach (var t in loaded)
            tasks.Add(t);
    }

    private void OnAddTaskClicked(object sender, EventArgs e)
    {
        editingTask = null;
        ShowTaskForm();
    }

    private void ShowTaskForm()
    {
        taskForm.IsVisible = true;
        taskCollectionView.IsEnabled = false;

        if (editingTask == null)
        {
            titleEntry.Text = string.Empty;
            descriptionEntry.Text = string.Empty;
            datePicker.Date = DateTime.Today;
            timePicker.Time = TimeSpan.Zero;
        }
        else
        {
            titleEntry.Text = editingTask.Title;
            descriptionEntry.Text = editingTask.Description;
            datePicker.Date = DateTime.TryParse(editingTask.Date, out var d) ? d : DateTime.Today;
            timePicker.Time = TimeSpan.TryParse(editingTask.Time, out var t) ? t : TimeSpan.Zero;
        }
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        taskForm.IsVisible = false;
        taskCollectionView.IsEnabled = true;
        editingTask = null;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(titleEntry.Text))
        {
            await DisplayAlert("Hata", "Başlık boş olamaz.", "Tamam");
            return;
        }

        saveBtn.IsEnabled = false;

        if (editingTask == null)
        {
            var newTask = new TodoTask
            {
                Title = titleEntry.Text.Trim(),
                Description = descriptionEntry.Text?.Trim(),
                Date = datePicker.Date.ToString("dd/MM/yyyy"),
                Time = timePicker.Time.ToString(@"hh\:mm"),
                IsCompleted = false
            };

            tasks.Add(newTask);
            await notlarService.AddTask(newTask);
        }
        else
        {
            editingTask.Title = titleEntry.Text.Trim();
            editingTask.Description = descriptionEntry.Text?.Trim();
            editingTask.Date = datePicker.Date.ToString("dd/MM/yyyy");
            editingTask.Time = timePicker.Time.ToString(@"hh\:mm");

            await notlarService.UpdateTask(editingTask);
        }

        saveBtn.IsEnabled = true;
        taskForm.IsVisible = false;
        taskCollectionView.IsEnabled = true;
        editingTask = null;
    }

    private async void OnDeleteTaskClicked(object sender, EventArgs e)
    {
        if (((Button)sender).BindingContext is TodoTask task)
        {
            bool confirm = await DisplayAlert("Silinsin mi?", $"'{task.Title}' silinsin mi?", "Evet", "Hayır");
            if (confirm)
            {
                tasks.Remove(task);
                await notlarService.DeleteTask(task);
            }
        }
    }

    private async void OnTaskCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox cb && cb.BindingContext is TodoTask task)
        {
            task.IsCompleted = e.Value;
            await notlarService.UpdateTask(task);
        }
    }

    private void OnEditTaskClicked(object sender, EventArgs e)
    {
        if (((Button)sender).BindingContext is TodoTask task)
        {
            editingTask = task;
            ShowTaskForm();
        }
    }
}
