import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Owner, CreateOwner, UpdateOwner } from '../../models/owner.model';

@Component({
  selector: 'app-owner-dialog',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './owner-dialog.component.html'
})
export class OwnerDialogComponent {
  @Input() owners: Owner[] = [];
  @Input() isOpen = false;

  @Output() close = new EventEmitter<void>();
  @Output() createOwner = new EventEmitter<CreateOwner>();
  @Output() updateOwner = new EventEmitter<{ id: number; owner: UpdateOwner }>();
  @Output() deleteOwner = new EventEmitter<number>();

  editingOwner: Owner | null = null;
  showAddForm = false;

  newOwner = {
    name: '',
    title: ''
  };

  editForm = {
    name: '',
    title: ''
  };

  startAdd(): void {
    this.showAddForm = true;
    this.editingOwner = null;
    this.newOwner = { name: '', title: '' };
  }

  cancelAdd(): void {
    this.showAddForm = false;
    this.newOwner = { name: '', title: '' };
  }

  onAdd(): void {
    if (this.newOwner.name.trim()) {
      this.createOwner.emit({
        name: this.newOwner.name,
        title: this.newOwner.title
      });
      this.cancelAdd();
    }
  }

  startEdit(owner: Owner): void {
    this.editingOwner = owner;
    this.showAddForm = false;
    this.editForm = {
      name: owner.name,
      title: owner.title
    };
  }

  cancelEdit(): void {
    this.editingOwner = null;
  }

  onSaveEdit(): void {
    if (this.editingOwner && this.editForm.name.trim()) {
      this.updateOwner.emit({
        id: this.editingOwner.id,
        owner: {
          name: this.editForm.name,
          title: this.editForm.title
        }
      });
      this.editingOwner = null;
    }
  }

  onDelete(owner: Owner): void {
    if (confirm(`Delete owner "${owner.name}"?`)) {
      this.deleteOwner.emit(owner.id);
    }
  }

  onClose(): void {
    this.close.emit();
  }
}
