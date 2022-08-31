using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Data;

namespace zwift
{
    public class WorkoutsViewModel : INotifyPropertyChanged // : ViewModelBase
    {
        private ObservableCollection<INavigationAware> _items;
        private INavigationAware _selectedItem;
        private string _selectedSort;
        private object _itemsLock = new object();

        public WorkoutCollectionRepository CollectionRepository { get; }
        public WorkoutCollection Custom { get; }
        public WorkoutCollection Favorites { get; }
        public WorkoutRepository WorkoutRepository { get; }
        public WorkoutCollection Plans { get; } // todo: implement

        public ObservableCollection<INavigationAware> Items
        {
            get => _items;
            set => SetField(ref _items, value);
        }
        public INavigationAware SelectedItem
        {
            get => _selectedItem;
            set => SetField(ref _selectedItem, value);
        }

        public WorkoutsViewModel()
        {
            WorkoutRepository = new WorkoutRepository
            {
                new Workout { Name = "workout 1", IsFavorite = false, IsCustom = false, Id = "1"},
                new Workout { Name = "workout 2", IsFavorite = false, IsCustom = false, Id = "1"},
                new Workout { Name = "workout 3", IsFavorite = true, IsCustom = false, Id = "2"},
                new Workout { Name = "workout 4", IsFavorite = true, IsCustom = false, Id = "2"},
                new Workout { Name = "workout 5", IsFavorite = true, IsCustom = false, Id = "3"},
                new Workout { Name = "workout 6", IsFavorite = false, IsCustom = false, Id = "3"},
                new Workout { Name = "workout 7", IsFavorite = false, IsCustom = false, Id = "3"},
                new Workout { Name = "workout 8", IsFavorite = false, IsCustom = true, Id = "4"},
                new Workout { Name = "workout 9", IsFavorite = false, IsCustom = true, Id = "4"},
            };

            CollectionRepository = new WorkoutCollectionRepository
            {
                new WorkoutCollection("Workout Collection 1", "1", WorkoutRepository.Where(f => f.Id == "1")),
                new WorkoutCollection("Workout Collection 2", "2", WorkoutRepository.Where(f => f.Id == "2")),
                new WorkoutCollection("Workout Collection 3", "3", WorkoutRepository.Where(f => f.Id == "3")),
                new WorkoutCollection("Workout Collection 4", "4", WorkoutRepository.Where(f => f.Id == "4")),
            };

            Custom = new WorkoutCollection("Custom", "5", WorkoutRepository.Where(f => f.IsCustom));
            Favorites = new WorkoutCollection("Favorites", "6", WorkoutRepository.Where(f => f.IsFavorite));
            Plans = new WorkoutCollection("Plans", "7", new List<Workout>(0));

            Items = new ObservableCollection<INavigationAware>
            {
                CollectionRepository,
                Custom,
                Favorites,
                WorkoutRepository
            };

            SelectedItem = Items[0];
        }
        
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }

    public interface INavigationAware
    {
        string Name { get; }
        string Description { get; }
    }


    public class WorkoutCollection : ObservableCollection<Workout>, INavigationAware
    {
        public string Name { get; }
        public string Description => $"{this.Count} Workouts";
        public string Id { get; }

        public WorkoutCollection(string name, string id, IEnumerable<Workout> workouts) : base(workouts)
        {
            Name = name;
            Id = id;
        }
    }

    // contains all collection 
    public class WorkoutCollectionRepository : ObservableCollection<WorkoutCollection>, INavigationAware
    {
        public string Name => "Collections";

        public string Description => $"{this.Count} Collections";
    }

    // contains all workouts
    public class WorkoutRepository : ObservableCollection<Workout>, INavigationAware
    {
        public string Name => "All Workouts";
        public string Description => $"{this.Count} Workouts";
    }

    public class Workout
    {
        public string Name { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsCustom { get; set; }
        public string Id { get; set; }
    }
}
