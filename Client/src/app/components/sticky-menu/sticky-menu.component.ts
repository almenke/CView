import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Project } from '../../models/project.model';

@Component({
  selector: 'app-sticky-menu',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './sticky-menu.component.html'
})
export class StickyMenuComponent {
  @Input() projects: Project[] = [];
  @Input() selectedProject: Project | null = null;

  @Output() projectSelected = new EventEmitter<Project>();
  @Output() editProjectClicked = new EventEmitter<void>();
  @Output() manageOwnersClicked = new EventEmitter<void>();
  @Output() manageSprintsClicked = new EventEmitter<void>();
  @Output() uploadExcelClicked = new EventEmitter<void>();
  @Output() newProjectClicked = new EventEmitter<void>();

  showProjectDropdown = false;

  toggleProjectDropdown(): void {
    this.showProjectDropdown = !this.showProjectDropdown;
  }

  selectProject(project: Project): void {
    this.projectSelected.emit(project);
    this.showProjectDropdown = false;
  }

  onEditProject(): void {
    this.editProjectClicked.emit();
  }

  onManageOwners(): void {
    this.manageOwnersClicked.emit();
  }

  onManageSprints(): void {
    this.manageSprintsClicked.emit();
  }

  onUploadExcel(): void {
    this.uploadExcelClicked.emit();
  }

  onNewProject(): void {
    this.newProjectClicked.emit();
  }
}
