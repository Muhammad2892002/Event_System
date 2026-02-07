import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-delete-confirmation',
  imports: [],
  templateUrl: './delete-confirmation.html',
  styleUrl: './delete-confirmation.css',
})
export class DeleteConfirmation {
  @Input() title:string="";
  @Input() content:string="";
  @Output() confirm=new EventEmitter<boolean>();
  deleteConfirm(confirm:boolean){
    this.confirm.emit(confirm);


  }

}
