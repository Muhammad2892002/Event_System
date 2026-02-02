import { Component } from '@angular/core';
import { FormControl, FormGroup,ReactiveFormsModule } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';

@Component({
  selector: 'app-requests-to-join-event',
  imports: [ReactiveFormsModule,NgxPaginationModule],
  templateUrl: './requests-to-join-event.html',
  styleUrl: './requests-to-join-event.css',
})
export class RequestsToJoinEvent {

 requestToJoinEvent:FormGroup=new FormGroup({
  eventId:new FormControl<number | null>(null),
  userId:new FormControl<number | null>(null),
  status:new FormControl<number | null>(null),
  requestedAt:new FormControl<string | null>(null),
 
  });

   paginationConfig={
    itemsPerPage:5,
    currentPage:1,
  }
  changePage(pageNumber:number){
    this.paginationConfig.currentPage=pageNumber;
  }

  click(){
    console.log(this.requestToJoinEvent.value);

  }

}
