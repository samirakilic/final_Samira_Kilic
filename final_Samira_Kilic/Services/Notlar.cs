using System.Text.Json;
using final_Samira_Kilic.Model;

namespace final_Samira_Kilic
{
    public class Notlar
    {
        private readonly string dosyaYolu = Path.Combine(FileSystem.AppDataDirectory, "gorevler.json");

        public async Task<List<TodoTask>> GetTasks()
        {
            try
            {
                if (!File.Exists(dosyaYolu))
                    return new List<TodoTask>();

                var json = await File.ReadAllTextAsync(dosyaYolu);
                return JsonSerializer.Deserialize<List<TodoTask>>(json) ?? new List<TodoTask>();
            }
            catch
            {
                return new List<TodoTask>();
            }
        }

        public async Task AddTask(TodoTask task)
        {
            var list = await GetTasks();
            list.Add(task);
            await SaveTasks(list);
        }

        public async Task UpdateTask(TodoTask updatedTask)
        {
            var list = await GetTasks();
            var existing = list.FirstOrDefault(t => t.Title == updatedTask.Title && t.Date == updatedTask.Date);
            if (existing != null)
            {
                existing.Description = updatedTask.Description;
                existing.Time = updatedTask.Time;
                existing.IsCompleted = updatedTask.IsCompleted;
            }
            await SaveTasks(list);
        }

        public async Task DeleteTask(TodoTask task)
        {
            var list = await GetTasks();
            var toRemove = list.FirstOrDefault(t => t.Title == task.Title && t.Date == task.Date);
            if (toRemove != null)
            {
                list.Remove(toRemove);
                await SaveTasks(list);
            }
        }

        private async Task SaveTasks(List<TodoTask> tasks)
        {
            var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(dosyaYolu, json);
        }
    }
}
