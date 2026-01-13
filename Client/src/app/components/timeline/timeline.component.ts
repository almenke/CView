import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Project } from '../../models/project.model';
import { Sprint } from '../../models/sprint.model';
import { Task } from '../../models/task.model';

interface MonthColumn {
  name: string;
  year: number;
  month: number;
  sprints: SprintColumn[];
}

interface SprintColumn {
  sprint: Sprint;
  tasks: Task[];
}

@Component({
  selector: 'app-timeline',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './timeline.component.html'
})
export class TimelineComponent implements OnChanges {
  @Input() project: Project | null = null;

  monthColumns: MonthColumn[] = [];

  // Color palette for task cards
  private taskColors = [
    'bg-red-300',
    'bg-blue-300',
    'bg-yellow-300',
    'bg-green-300',
    'bg-purple-300',
    'bg-pink-300',
    'bg-indigo-300',
    'bg-orange-300',
    'bg-teal-300',
    'bg-cyan-300'
  ];

  // Map task names to colors for consistency
  private taskColorMap = new Map<string, string>();

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['project']) {
      this.buildTimeline();
    }
  }

  buildTimeline(): void {
    if (!this.project) {
      this.monthColumns = [];
      return;
    }

    // Get unique months from project date range (up to 6 months)
    const startDate = new Date(this.project.startsAt);
    const endDate = new Date(this.project.endsAt);

    this.monthColumns = [];
    const current = new Date(startDate.getFullYear(), startDate.getMonth(), 1);
    const maxMonths = 6;
    let monthCount = 0;

    while (current <= endDate && monthCount < maxMonths) {
      const monthSprints = this.getSprintsForMonth(current.getFullYear(), current.getMonth());

      this.monthColumns.push({
        name: this.getMonthName(current.getMonth()),
        year: current.getFullYear(),
        month: current.getMonth(),
        sprints: monthSprints.map(sprint => ({
          sprint,
          tasks: this.getTasksForSprint(sprint)
        }))
      });

      current.setMonth(current.getMonth() + 1);
      monthCount++;
    }
  }

  getSprintsForMonth(year: number, month: number): Sprint[] {
    if (!this.project) return [];

    return this.project.sprints.filter(sprint => {
      const sprintStart = new Date(sprint.startsAt);
      const displayMonth = this.getDisplayMonth(sprintStart);
      return displayMonth.year === year && displayMonth.month === month;
    });
  }

  // If a sprint starts within 5 days of the next month, display it in the next month
  private getDisplayMonth(sprintStart: Date): { year: number; month: number } {
    const year = sprintStart.getFullYear();
    const month = sprintStart.getMonth();
    const day = sprintStart.getDate();

    // Get the last day of the current month
    const lastDayOfMonth = new Date(year, month + 1, 0).getDate();

    // If sprint starts within 5 days of month end, shift to next month
    if (day > lastDayOfMonth - 5) {
      const nextMonth = new Date(year, month + 1, 1);
      return { year: nextMonth.getFullYear(), month: nextMonth.getMonth() };
    }

    return { year, month };
  }

  getTasksForSprint(sprint: Sprint): Task[] {
    if (!this.project) return [];

    const sprintStart = new Date(sprint.startsAt);
    const sprintEnd = new Date(sprint.endsAt);

    return this.project.tasks.filter(task => {
      const taskStart = new Date(task.plannedStartsAt);
      // Task starts within this sprint
      return taskStart >= sprintStart && taskStart <= sprintEnd;
    }).sort((a, b) =>
      new Date(a.plannedStartsAt).getTime() - new Date(b.plannedStartsAt).getTime()
    );
  }

  getMonthName(month: number): string {
    const months = ['January', 'February', 'March', 'April', 'May', 'June',
                    'July', 'August', 'September', 'October', 'November', 'December'];
    return months[month];
  }

  formatSprintDates(sprint: Sprint): string {
    const start = new Date(sprint.startsAt);
    const end = new Date(sprint.endsAt);
    return `${start.getMonth() + 1}/${start.getDate()} - ${end.getMonth() + 1}/${end.getDate()}`;
  }

  formatTaskDates(task: Task): string {
    const start = new Date(task.plannedStartsAt);
    const end = new Date(task.plannedEndsAt);
    return `${start.getMonth() + 1}/${start.getDate()}/${start.getFullYear().toString().slice(-2)} - ${end.getMonth() + 1}/${end.getDate()}/${end.getFullYear().toString().slice(-2)}`;
  }

  getTaskColor(task: Task): string {
    // Get base task name (before " - Dev" or " - UAT" etc.)
    const baseName = this.getBaseTaskName(task.name);

    if (!this.taskColorMap.has(baseName)) {
      const colorIndex = this.taskColorMap.size % this.taskColors.length;
      this.taskColorMap.set(baseName, this.taskColors[colorIndex]);
    }

    return this.taskColorMap.get(baseName) || this.taskColors[0];
  }

  getBaseTaskName(name: string): string {
    // Remove common suffixes like " - Dev", " - UAT", " - QA", etc.
    return name.replace(/\s*-\s*(Dev|UAT|QA|Test|Prod|Production)$/i, '').trim();
  }
}
