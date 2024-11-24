import { NgIf } from "@angular/common";
import { Component, Input, Output, EventEmitter } from "@angular/core";

@Component({
  imports: [NgIf],
  selector: "app-delete-confirmation-modal",
  templateUrl: "./delete-confirmation-modal.component.html",
  styleUrls: ["./delete-confirmation-modal.component.css"],
  standalone: true,
})
export class DeleteConfirmationModalComponent {
  @Input() itemName: string = "";
  @Input() isVisible: boolean = false;
  @Output() confirm = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  closeModal() {
    this.cancel.emit();
  }

  confirmDeletion() {
    this.confirm.emit();
  }
}
