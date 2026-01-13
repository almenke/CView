import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StickyMenuComponent } from './components/sticky-menu/sticky-menu.component';
import { TimelineComponent } from './components/timeline/timeline.component';
import { ProjectDialogComponent } from './components/project-dialog/project-dialog.component';
import { OwnerDialogComponent } from './components/owner-dialog/owner-dialog.component';
import { SprintDialogComponent } from './components/sprint-dialog/sprint-dialog.component';
import { FileUploadComponent } from './components/file-upload/file-upload.component';
import { ProjectService } from './services/project.service';
import { OwnerService } from './services/owner.service';
import { SprintService } from './services/sprint.service';
import { Project, CreateProject, UpdateProject } from './models/project.model';
import { Owner, CreateOwner, UpdateOwner } from './models/owner.model';
import { Sprint, CreateSprint, UpdateSprint } from './models/sprint.model';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    StickyMenuComponent,
    TimelineComponent,
    ProjectDialogComponent,
    OwnerDialogComponent,
    SprintDialogComponent,
    FileUploadComponent
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  projects: Project[] = [];
  selectedProject: Project | null = null;

  // Dialog states
  showProjectDialog = false;
  showOwnerDialog = false;
  showSprintDialog = false;
  showFileUpload = false;
  isUploading = false;

  // For editing
  editingProject: Project | null = null;

  constructor(
    private projectService: ProjectService,
    private ownerService: OwnerService,
    private sprintService: SprintService
  ) {}

  ngOnInit(): void {
    this.loadProjects();
  }

  loadProjects(): void {
    this.projectService.getAll().subscribe({
      next: (projects) => {
        this.projects = projects;
        if (this.selectedProject) {
          // Refresh selected project
          const updated = projects.find(p => p.id === this.selectedProject!.id);
          if (updated) {
            this.selectedProject = updated;
          }
        } else if (projects.length > 0) {
          this.selectProject(projects[0]);
        }
      },
      error: (err) => console.error('Error loading projects:', err)
    });
  }

  selectProject(project: Project): void {
    this.projectService.getById(project.id).subscribe({
      next: (fullProject) => {
        this.selectedProject = fullProject;
      },
      error: (err) => console.error('Error loading project:', err)
    });
  }

  // Project Dialog
  onNewProject(): void {
    this.editingProject = null;
    this.showProjectDialog = true;
  }

  onEditProject(): void {
    this.editingProject = this.selectedProject;
    this.showProjectDialog = true;
  }

  onSaveProject(data: CreateProject | UpdateProject): void {
    if (this.editingProject) {
      this.projectService.update(this.editingProject.id, data as UpdateProject).subscribe({
        next: () => {
          this.showProjectDialog = false;
          this.loadProjects();
        },
        error: (err) => console.error('Error updating project:', err)
      });
    } else {
      this.projectService.create(data as CreateProject).subscribe({
        next: (newProject) => {
          this.showProjectDialog = false;
          this.loadProjects();
          this.selectProject(newProject);
        },
        error: (err) => console.error('Error creating project:', err)
      });
    }
  }

  onDeleteProject(id: number): void {
    this.projectService.delete(id).subscribe({
      next: () => {
        this.showProjectDialog = false;
        this.selectedProject = null;
        this.loadProjects();
      },
      error: (err) => console.error('Error deleting project:', err)
    });
  }

  // Owner Dialog
  onManageOwners(): void {
    this.showOwnerDialog = true;
  }

  onCreateOwner(owner: CreateOwner): void {
    if (!this.selectedProject) return;
    this.ownerService.create(this.selectedProject.id, owner).subscribe({
      next: () => this.refreshSelectedProject(),
      error: (err) => console.error('Error creating owner:', err)
    });
  }

  onUpdateOwner(data: { id: number; owner: UpdateOwner }): void {
    this.ownerService.update(data.id, data.owner).subscribe({
      next: () => this.refreshSelectedProject(),
      error: (err) => console.error('Error updating owner:', err)
    });
  }

  onDeleteOwner(id: number): void {
    this.ownerService.delete(id).subscribe({
      next: () => this.refreshSelectedProject(),
      error: (err) => console.error('Error deleting owner:', err)
    });
  }

  // Sprint Dialog
  onManageSprints(): void {
    this.showSprintDialog = true;
  }

  onCreateSprint(sprint: CreateSprint): void {
    if (!this.selectedProject) return;
    this.sprintService.create(this.selectedProject.id, sprint).subscribe({
      next: () => this.refreshSelectedProject(),
      error: (err) => console.error('Error creating sprint:', err)
    });
  }

  onUpdateSprint(data: { id: number; sprint: UpdateSprint }): void {
    this.sprintService.update(data.id, data.sprint).subscribe({
      next: () => this.refreshSelectedProject(),
      error: (err) => console.error('Error updating sprint:', err)
    });
  }

  onDeleteSprint(id: number): void {
    this.sprintService.delete(id).subscribe({
      next: () => this.refreshSelectedProject(),
      error: (err) => console.error('Error deleting sprint:', err)
    });
  }

  onRegenerateSprints(): void {
    if (!this.selectedProject) return;
    this.projectService.regenerateSprints(this.selectedProject.id).subscribe({
      next: () => this.refreshSelectedProject(),
      error: (err) => console.error('Error regenerating sprints:', err)
    });
  }

  // File Upload
  onUploadExcel(): void {
    this.showFileUpload = true;
  }

  onFileSelected(file: File): void {
    if (!this.selectedProject) return;

    this.isUploading = true;
    this.projectService.importExcel(this.selectedProject.id, file).subscribe({
      next: (result) => {
        this.isUploading = false;
        this.showFileUpload = false;
        if (result.success) {
          alert(`Import successful! ${result.tasksImported} tasks and ${result.ownersImported} owners imported.`);
        } else {
          alert(`Import completed with errors:\n${result.errors.join('\n')}`);
        }
        this.refreshSelectedProject();
      },
      error: (err) => {
        this.isUploading = false;
        console.error('Error importing file:', err);
        alert('Error importing file. Please try again.');
      }
    });
  }

  refreshSelectedProject(): void {
    if (this.selectedProject) {
      this.selectProject(this.selectedProject);
    }
  }
}
