import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Project, CreateProject, UpdateProject } from '../../models/project.model';
import { Owner } from '../../models/owner.model';

@Component({
  selector: 'app-project-dialog',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './project-dialog.component.html'
})
export class ProjectDialogComponent implements OnInit {
  @Input() project: Project | null = null;
  @Input() owners: Owner[] = [];
  @Input() isOpen = false;

  @Output() close = new EventEmitter<void>();
  @Output() save = new EventEmitter<CreateProject | UpdateProject>();
  @Output() deleteProject = new EventEmitter<number>();
  @Output() clearData = new EventEmitter<number>();

  formData = {
    name: '',
    ownerId: null as number | null,
    startsAt: '',
    endsAt: '',
    sprintStartNumber: 1
  };

  get isEditMode(): boolean {
    return this.project !== null;
  }

  ngOnInit(): void {
    this.resetForm();
  }

  ngOnChanges(): void {
    this.resetForm();
  }

  resetForm(): void {
    if (this.project) {
      this.formData = {
        name: this.project.name,
        ownerId: this.project.ownerId || null,
        startsAt: this.formatDate(new Date(this.project.startsAt)),
        endsAt: this.formatDate(new Date(this.project.endsAt)),
        sprintStartNumber: 1
      };
    } else {
      const today = new Date();
      const sixMonthsLater = new Date();
      sixMonthsLater.setMonth(sixMonthsLater.getMonth() + 6);

      this.formData = {
        name: '',
        ownerId: null,
        startsAt: this.formatDate(today),
        endsAt: this.formatDate(sixMonthsLater),
        sprintStartNumber: 1
      };
    }
  }

  formatDate(date: Date): string {
    return date.toISOString().split('T')[0];
  }

  onSave(): void {
    const data = {
      name: this.formData.name,
      ownerId: this.formData.ownerId || undefined,
      startsAt: new Date(this.formData.startsAt),
      endsAt: new Date(this.formData.endsAt),
      sprintStartNumber: this.formData.sprintStartNumber
    };
    this.save.emit(data);
  }

  onDelete(): void {
    if (this.project && confirm('Are you sure you want to delete this project?')) {
      this.deleteProject.emit(this.project.id);
    }
  }

  onClearData(): void {
    if (this.project && confirm('Clear all tasks and owners for this project? This cannot be undone.')) {
      this.clearData.emit(this.project.id);
    }
  }

  onClose(): void {
    this.close.emit();
  }
}
