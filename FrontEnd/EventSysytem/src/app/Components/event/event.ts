import { NgClass } from '@angular/common';
import { Component } from '@angular/core';
import { NgxPaginationModule } from 'ngx-pagination';
import { FormGroup,FormControlName,ReactiveFormsModule,FormControl} from '@angular/forms';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-event',
  imports: [ReactiveFormsModule, NgClass, NgxPaginationModule],
  templateUrl: './event.html',
  styleUrl: './event.css',
})
export class EventComponent {
  searchEventForm:FormGroup=new FormGroup({
    title:new FormControl<string | null>(null),
    eventDate:new FormControl<string | null>(null),
    organizerId:new FormControl<number | null>(null),
    createdAt:new FormControl<string | null>(null),
    LocationId:new FormControl<number | null>(null),
 
  });
   paginationConfig={
    itemsPerPage:5,
    currentPage:1,
  }
  changePage(pageNumber:number){
    this.paginationConfig.currentPage=pageNumber;
  }

  click(){
    console.log(this.searchEventForm.value);

  }

}
