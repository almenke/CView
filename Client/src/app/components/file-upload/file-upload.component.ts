import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-file-upload',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './file-upload.component.html'
})
export class FileUploadComponent {
  @Input() isOpen = false;
  @Input() isLoading = false;

  @Output() close = new EventEmitter<void>();
  @Output() fileSelected = new EventEmitter<File>();

  selectedFile: File | null = null;
  dragOver = false;

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    this.dragOver = true;
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    this.dragOver = false;
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    this.dragOver = false;
    const files = event.dataTransfer?.files;
    if (files && files.length > 0) {
      this.handleFile(files[0]);
    }
  }

  onFileInputChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.handleFile(input.files[0]);
    }
  }

  handleFile(file: File): void {
    const validExtensions = ['.xlsx', '.xls'];
    const fileName = file.name.toLowerCase();
    if (validExtensions.some(ext => fileName.endsWith(ext))) {
      this.selectedFile = file;
    } else {
      alert('Please select an Excel file (.xlsx or .xls)');
    }
  }

  onUpload(): void {
    if (this.selectedFile) {
      this.fileSelected.emit(this.selectedFile);
    }
  }

  onClose(): void {
    this.selectedFile = null;
    this.close.emit();
  }
}
