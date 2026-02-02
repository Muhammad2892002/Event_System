import { Component } from '@angular/core';
import { FormControl, FormGroup,ReactiveFormsModule } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';

@Component({
  selector: 'app-request-to-be-organizer',
  imports: [ReactiveFormsModule,NgxPaginationModule],
  templateUrl: './request-to-be-organizer.html',
  styleUrl: './request-to-be-organizer.css',
})
export class RequestToBeOrganizer {

   searchRequestForm:FormGroup=new FormGroup({
    status:new FormControl<number | null>(null),
    requestedTime:new FormControl<string | null>(null),
   
 
  });

   paginationConfig={
    itemsPerPage:5,
    currentPage:1,
  }
  changePage(pageNumber:number){
    this.paginationConfig.currentPage=pageNumber;
  }

  click(){
    console.log(this.searchRequestForm.value);

  }


}
