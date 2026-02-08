import { Component } from '@angular/core';
import { EventRequest } from '../../Interface/event-request';
import { showAllOragnizerRequest } from '../../Interface/show-organizers-request';
import { RequestToJoinEventService } from '../../Services/request-to-join-event-service';

import { RequestToBeOrganizerService } from '../../Services/request-to-be-organizer';
import { FormControl, FormGroup,ReactiveFormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { AddAndUpdateRequest } from '../../Interface/AddRequestAndUpdate';
import { EventService } from '../../Services/event-service';
import { EventObj } from '../../Interface/EventObj';

@Component({
  selector: 'app-home',
  imports: [DatePipe],
  templateUrl: './home.html',
  styleUrl: './home.css',
})

export class HomeComponent {
  constructor(private requestToJoinEvent:RequestToJoinEventService,
    private requestToBeOrganizer:RequestToBeOrganizerService,
    private eventService :EventService
  ){}


   getAllPendingRequestToEvents:FormGroup=new FormGroup({
  eventId:new FormControl<number | null>(null),
  userId:new FormControl<number | null>(null),
  status:new FormControl<number | null>(null),
  requestedAt:new FormControl<string | null>(null),
 
  });

  UpdateRequest:FormGroup=new FormGroup({
    Id:new FormControl<number|null>(null),
    EventId:new FormControl<number|null>(null),
    UserId:new FormControl<number|null>(null),
    Status:new FormControl<number|null>(null),
    RequestedAt:new FormControl<number|null>(null),



  });
//
  sendJoinRequestResult(item:any,statusObj:boolean){
    let result:AddAndUpdateRequest={
      id:item.id,
      EventId:item.eventId,
      UserId:item.userId,
      status:statusObj?2:1
      


    };
    this.requestToJoinEvent.EditRequestToJoinEvent(result).subscribe({

      next:(res:any)=>{
        alert("Request changed");
      },
      error:err=>{
        console.log(err.err||err.error);
      }
    })


 
    console.log(result);
 
  }


    sendJoinRequestToBeOrganizerResult(item:showAllOragnizerRequest,statusObj:boolean){
    let result:showAllOragnizerRequest={
      id:item.id,
      statusId:statusObj?2:1
      


    };
    this.requestToBeOrganizer.UpdateRequest(result).subscribe({

      next:(res:any)=>{
        alert("Request changed");
      },
      error:err=>{
        console.log(err.err||err.error);
      }
    })


 
    console.log(result);
 
  }

  ngOnInit(){
    this.getAllPendingRequestToJoinEvent();
    this.getAllPendingRequestToAnOrganizer();
    this.getAllEvents();

  }
     getAllPendingRequestsToBeAnOrganizer:FormGroup=new FormGroup({
    status:new FormControl<number | null>(null),
    requestedTime:new FormControl<string | null>(null),
    
   
 
  });

  pendingRequestToJoinEvent:EventRequest[]=[];
  pendingRequestToBeOrganizer:showAllOragnizerRequest[]=[];

  getAllPendingRequestToJoinEvent(){
    this.getAllPendingRequestToEvents.patchValue({
      status:0
    });
      
    
    
    
    this.requestToJoinEvent.GetAllRequest(this.getAllPendingRequestToEvents.value).subscribe({

      next:(res:any)=>{
        res.forEach((element:any)=>{
          let request:EventRequest={
            id:element.id,
            eventId:element.eventId,
            userId:element.userId,
            eventName:element.eventName,
            userName:element.name,
            email:element.email,
            status:element.status,
            statusName:element.statusName,
            requestedAt:element.requestedAt

          }
          this.pendingRequestToJoinEvent.push(request);

        })

      },
      error:(err:any)=>{
        console.log(err);
      }
    });


  }



   getAllPendingRequestToAnOrganizer(){
    this.getAllPendingRequestsToBeAnOrganizer.patchValue({
      status:0
    });
      
    
    
    
     this.requestToBeOrganizer.GetAllRequests(this.getAllPendingRequestsToBeAnOrganizer.value).subscribe({
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
          this.pendingRequestToBeOrganizer.push(request);


        })
      },
      error:err=>{
        console.log(err);
      }

      
    })


  }

  AllEvents : EventObj[]=[];

  getAllEvents(){
    this.eventService.getAllEvents(null).subscribe({
      next:(res:any)=>{
        
        res.forEach((x:any)=>{
         let eventobj:EventObj={
          Id:x.id,
          Title:x.title,
          Description:x.description,
          Location:x.location,
          EventDate:x.eventDate,
          OrganizerId:x.organizerId,
          maxParticipants:x.maxParticipants,
          OrganizerName:x.organizerName,
          LocationName:x.locationName

         }
         this.AllEvents.push(eventobj);
          

        })

      },
      error:err=>{}
    })
  }

}
