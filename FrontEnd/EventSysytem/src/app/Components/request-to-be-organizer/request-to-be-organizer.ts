import { Component } from '@angular/core';
import { FormControl, FormGroup,ReactiveFormsModule } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { showAllOragnizerRequest } from '../../Interface/show-organizers-request';
import { RequestToBeOrganizerService } from '../../Services/request-to-be-organizer';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-request-to-be-organizer',
  imports: [ReactiveFormsModule,NgxPaginationModule,DatePipe],
  templateUrl: './request-to-be-organizer.html',
  styleUrl: './request-to-be-organizer.css',
})

export class RequestToBeOrganizer {
  constructor(private organizerService:RequestToBeOrganizerService){}

   allOrganizers:showAllOragnizerRequest[]=[];

   searchRequestForm:FormGroup=new FormGroup({
    status:new FormControl<number | null>(null),
    requestedTime:new FormControl<string | null>(null),
    
   
 
  });

   ngOnInit(){
    this.click();//load all events at the beginning
   


  }
  

   paginationConfig={
    itemsPerPage:5,
    currentPage:1,
  }
  changePage(pageNumber:number){
    this.paginationConfig.currentPage=pageNumber;
  }

  click(){
    this.allOrganizers=[];
    this.organizerService.GetAllRequests(this.searchRequestForm.value).subscribe({
      next:(res:any)=>{
        res.forEach((x:any)=>{
          let request:showAllOragnizerRequest={
            id:x.id,
            userId:x.userId,
            attendeeName:x.name,
            statusId:x.status,
            statusName:x.statusName,
            RequestDate:x.requestedAt
            


          }
          this.allOrganizers.push(request);


        })
      },
      error:err=>{
        console.log(err);
      }

      
    })
 

  }


}
