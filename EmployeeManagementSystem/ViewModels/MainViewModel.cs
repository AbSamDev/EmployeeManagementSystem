using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using EmployeeManagementSystem.Commands;
using EmployeeManagementSystem.Models;


namespace EmployeeManagementSystem.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Employee _currentEmployee;
        private ObservableCollection<Employee> _employees;

        public ICommand SaveCommand { get; private set; }
        public ICommand DisplayAllCommand { get; private set; }

        public List<string> Departments { get; } = new List<string>
    { "R&D", "QA", "Production", "Accounts", "Admin" };

        public List<string> Genders { get; } = new List<string>
    { "Male", "Female", "Other" };

        public Employee CurrentEmployee
        {
            get => _currentEmployee;
            set
            {
                _currentEmployee = value;
                OnPropertyChanged(nameof(CurrentEmployee));
            }
        }

        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }

        public MainViewModel()
        {
            CurrentEmployee = new Employee();
            Employees = new ObservableCollection<Employee>();
            SaveCommand = new RelayCommand(SaveEmployee);
            DisplayAllCommand = new RelayCommand(LoadEmployees);
        }

        private void SaveEmployee(object obj)
        {
            var csvLine = $"{CurrentEmployee.Name},{CurrentEmployee.FatherName},{CurrentEmployee.CNIC}," +
                          $"{CurrentEmployee.Designation},{CurrentEmployee.DateOfBirth:yyyy-MM-dd}," +
                          $"{CurrentEmployee.Gender},{CurrentEmployee.Department},{CurrentEmployee.IsManager}";

            File.AppendAllLines("employees.csv", new[] { csvLine });
            CurrentEmployee = new Employee();
        }

        private void LoadEmployees(object obj)
        {
            Employees.Clear();
            if (!File.Exists("employees.csv")) return;

            var lines = File.ReadAllLines("employees.csv");
            foreach (var line in lines)
            {
                var values = line.Split(',');
                Employees.Add(new Employee
                {
                    Name = values[0],
                    FatherName = values[1],
                    CNIC = values[2],
                    Designation = values[3],
                    DateOfBirth = DateTime.Parse(values[4]),
                    Gender = values[5],
                    Department = values[6],
                    IsManager = bool.Parse(values[7])
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
