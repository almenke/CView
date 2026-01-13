import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Sprint, CreateSprint, UpdateSprint } from '../../models/sprint.model';

@Component({
  selector: 'app-sprint-dialog',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './sprint-dialog.component.html'
})
export class SprintDialogComponent {
  @Input() sprints: Sprint[] = [];
  @Input() isOpen = false;

  @Output() close = new EventEmitter<void>();
  @Output() createSprint = new EventEmitter<CreateSprint>();
  @Output() updateSprint = new EventEmitter<{ id: number; sprint: UpdateSprint }>();
  @Output() deleteSprint = new EventEmitter<number>();
  @Output() regenerateSprints = new EventEmitter<void>();

  editingSprint: Sprint | null = null;
  showAddForm = false;

  newSprint = {
    name: '',
    startsAt: '',
    endsAt: ''
  };

  editForm = {
    name: '',
    startsAt: '',
    endsAt: ''
  };

  formatDate(date: Date | string): string {
    const d = new Date(date);
    return d.toISOString().split('T')[0];
  }

  formatDisplayDate(date: Date | string): string {
    const d = new Date(date);
    return d.toLocaleDateString('en-US', { month: 'short', day: 'numeric' });
  }

  startAdd(): void {
    this.showAddForm = true;
    this.editingSprint = null;
    const today = new Date();
    const twoWeeksLater = new Date();
    twoWeeksLater.setDate(twoWeeksLater.getDate() + 13);
    this.newSprint = {
      name: `Sprint ${this.sprints.length + 1}`,
      startsAt: this.formatDate(today),
      endsAt: this.formatDate(twoWeeksLater)
    };
  }

  cancelAdd(): void {
    this.showAddForm = false;
  }

  onAdd(): void {
    if (this.newSprint.name.trim()) {
      this.createSprint.emit({
        name: this.newSprint.name,
        startsAt: new Date(this.newSprint.startsAt),
        endsAt: new Date(this.newSprint.endsAt)
      });
      this.cancelAdd();
    }
  }

  startEdit(sprint: Sprint): void {
    this.editingSprint = sprint;
    this.showAddForm = false;
    this.editForm = {
      name: sprint.name,
      startsAt: this.formatDate(sprint.startsAt),
      endsAt: this.formatDate(sprint.endsAt)
    };
  }

  cancelEdit(): void {
    this.editingSprint = null;
  }

  onSaveEdit(): void {
    if (this.editingSprint && this.editForm.name.trim()) {
      this.updateSprint.emit({
        id: this.editingSprint.id,
        sprint: {
          name: this.editForm.name,
          startsAt: new Date(this.editForm.startsAt),
          endsAt: new Date(this.editForm.endsAt)
        }
      });
      this.editingSprint = null;
    }
  }

  onDelete(sprint: Sprint): void {
    if (confirm(`Delete sprint "${sprint.name}"?`)) {
      this.deleteSprint.emit(sprint.id);
    }
  }

  onRegenerate(): void {
    if (confirm('This will delete all existing sprints and regenerate them based on project dates. Continue?')) {
      this.regenerateSprints.emit();
    }
  }

  onClose(): void {
    this.close.emit();
  }
}
