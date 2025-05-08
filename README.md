# 🚀 CPU Scheduling Simulator

This project is a CPU scheduling simulator designed and implemented as a university project for the Operating Systems course.

## 📋 Project Features

- Implementation of various CPU scheduling algorithms (FCFS, SJF, RR, SRT)
- Graphical User Interface using WPF
- **Accurate Gantt chart visualization for all algorithms, including preemptive (RR, SRT)**
- Calculation of average turnaround and waiting times
- **Modern UI with vertical and horizontal scrolling support**
- **Flexible process input: auto-naming, minimum 10 rows, and validation**

## 🛠️ Implemented Algorithms

- **FCFS (First Come First Served)**
  - Scheduling based on process arrival order
  - Simplest scheduling algorithm

- **SJF (Shortest Job First)**
  - Scheduling based on shortest execution time
  - Optimal average waiting time

- **RR (Round Robin)**
  - Time-slice based preemptive scheduling
  - User-defined quantum
  - **Gantt chart now shows all execution intervals for each process**

- **SRT (Shortest Remaining Time)**
  - Preemptive version of SJF
  - **Gantt chart now shows all execution intervals for each process**

## 🎨 User Interface

- Process input table with auto-naming and validation
- Scheduling results table
- **Gantt chart for visual scheduling representation (shows all execution slices)**
- Display of average turnaround and waiting times
- **Full vertical scroll for the app and horizontal scroll for Gantt chart**

## 🚀 How to Run

### Method 1: Using Visual Studio
1. Open the project in Visual Studio
2. Build the project
3. Run the application

### Method 2: Using Shortcut
1. Double-click on the `CpuScheduling` shortcut
2. The application will start automatically

### Using the Application
1. Enter processes with the following specifications:
   - Process name (auto-filled, editable)
   - Arrival time
   - Burst time
2. Select the desired algorithm
3. View the results and Gantt chart

## 📊 Outputs

- Start and finish time for each process
- Turnaround time for each process
- Waiting time for each process
- Average turnaround and waiting times
- **Gantt chart for visual scheduling representation (shows all execution intervals, not just one per process)**

## 🆕 Technical Improvements (2025 Update)

- **GanttSlice model**: Enables accurate Gantt chart for preemptive algorithms
- **Improved Gantt chart rendering**: All execution intervals are shown for RR and SRT
- **UI improvements**: Full vertical scroll for the app, horizontal scroll for Gantt chart
- **Better process input handling**: Minimum 10 rows, auto-naming, and validation
- **Code documentation and refactoring**: Professional comments and code cleanup

## 👨‍💻 Developer

- **Name:** Mohammadreza Bonyadi
- **Course:** Operating Systems
- **University:** Islamic Azad University
- **Professor Name:** Mohammadhossein Shafiabadi

## 📦 Releases

You can download the latest stable release from the [Releases](https://github.com/Mohammadreza0083/CpuScheduling/releases) page.

### Latest Release: v1.0.0
- Initial release with FCFS and SJF algorithms
- Basic GUI implementation
- Gantt chart visualization

## 📚 Resources

- Operating System Concepts by Silberschatz
- Operating Systems course materials
- Online resources related to CPU scheduling

## 🔗 Quick Links
- [Download Latest Release](https://github.com/Mohammadreza0083/CpuScheduling/releases/latest)
- [View Source Code](https://github.com/Mohammadreza0083/CpuScheduling)
- [Report Issues](https://github.com/Mohammadreza0083/CpuScheduling/issues) 
