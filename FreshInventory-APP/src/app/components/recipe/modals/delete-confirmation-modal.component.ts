import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Recipe } from '../../../models/recipe.model';

@Component({
  selector: 'app-recipe-delete-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './delete-confirmation-modal.component.html',
  styleUrls: ['./delete-confirmation-modal.component.css']
})
export class RecipeDeleteModalComponent {
  @Input() recipe?: Recipe | null;
  @Output() confirm = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  onConfirm(): void {
    this.confirm.emit();
  }

  onCancel(): void {
    this.cancel.emit();
  }
}