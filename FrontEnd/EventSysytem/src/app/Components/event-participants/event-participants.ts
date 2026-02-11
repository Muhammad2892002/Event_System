import { Component, ElementRef, ViewChild } from '@angular/core';
import { NgxPaginationModule } from 'ngx-pagination';
import { FormGroup, FormControl,ReactiveFormsModule, Validators } from '@angular/forms';
import { Filter } from '../../Interface/Filter';
import { EventParticipantsService } from '../../Services/EventParticipantsService';
import { EventParticipant } from '../../Interface/event-participant';
import { DatePipe } from '@angular/common';
import { AttendeeService } from '../../Services/AttendeeService';
import { EventService } from '../../Services/event-service';
import { AddAndUpdateParticipant } from '../../Interface/AddParticipant';
import { DeleteConfirmation } from '../delete-confirmation/delete-confirmation';

@Component({
  selector: 'app-event-participants',
  imports: [NgxPaginationModule, ReactiveFormsModule,DatePipe,DeleteConfirmation],
  templateUrl: './event-participants.html',
  styleUrl: './event-participants.css',
})
export class EventParticipants {
  changePage(pageNumber: number) {
    this.paginationConfig.currentPage=pageNumber;
  }
  @ViewChild("closeDialog")closeButton!:ElementRef;
  constructor(private eventParticipantsService: EventParticipantsService
    ,private participantServ:AttendeeService, private eventServ: EventService) {}
  searchParticipants:FormGroup=new FormGroup({
    participantId:new FormControl<number | null>(null),
    eventId:new FormControl<number | null>(null),
  });

  addAndUpdateParticipant:FormGroup=new FormGroup({
    Id:new FormControl<number|null>(0),
    participantId:new FormControl<number>(0,Validators.required),
    eventId:new FormControl<number>(0,Validators.required)

  });
 
   


  participant:Filter[]=[];

  events:Filter[]=[];
  allEventParticipants:EventParticipant[]=[];
  ngOnInit(){
    this.loadParticipantsInEvent();
    this.loadEventsForParticipant();
    this.click();
  }
   paginationConfig={
    itemsPerPage:5,
    currentPage:1,
  }

  click(){
    this.allEventParticipants=[];
    this.eventParticipantsService.GetAllParticipants(this.searchParticipants.value).subscribe({
      next:(res:any)=>{
        res.forEach((x:any)=>{
          let participant:EventParticipant={
            Id:x.id,
            organizerId:x.organizerId,
            locationId:x.locationId,
            AttendeeId:x.attendeeId,
             EventId:x.eventId,
            AttendeeName:x.attendeeName,
           email:x.email,
            EventName:x.eventName,
            
            locationName:x.location,
            HeldAt:x.heldAt,
            organizerName:x.organizerName,
            
            
            
          };
          this.allEventParticipants.push(participant);
        })
        console.log(this.allEventParticipants);
      },
      error:(err:any)=>{console.log(err);
        console.log(err.error);
      }
    });

    console.log(this.searchParticipants.value);
  }
  loadParticipantsInEvent(){
    this.participant = [{Id:null,Name:'Choose Participant'},];
    this.eventParticipantsService.getAllParticipantsInEvent().subscribe({
      next:(res:any)=>{
        res.forEach((x:any)=>{
          let participant:Filter={Id:x.participantUserId,Name:x.participantName}
          this.participant.push(participant);
        })
       
      },
      error:(err:any)=>{
        console.log(err);
      }
    });
 
  }
loadEventsForParticipant(){
  this.events=[{Id:null,Name:'Choose Event'},];

   this.eventParticipantsService.getAllEventsInParticipant().subscribe({
      next:(res:any)=>{
        res.forEach((x:any)=>{
          let event:Filter={Id:x.eventId,Name:x.eventName}
          this.events.push(event);
        })
        
      },
      error:(err:any)=>{
        console.log(err);
      }
    });

}
  participants = [

  ];

  eventsName = [
  
  ];
  loadAddListsAndUpdate(){
    this.addAndUpdateParticipant.reset();
    this.allAtendee();
    this.LoadallEvents()
  }


  // FormGroup with two dropdowns
  participantEventForm: FormGroup = new FormGroup({
    participantId: new FormControl<number | null>(null, Validators.required),
    eventId: new FormControl<number | null>(null, Validators.required)
  });

    saveParticipantEvent() {
      let participant:AddAndUpdateParticipant={
        Id:this.addAndUpdateParticipant.value.Id,
        participantId:this.addAndUpdateParticipant.value.participantId,
        eventId:this.addAndUpdateParticipant.value.eventId,

      };

    if (this.addAndUpdateParticipant.valid) {
      if(participant.Id==null){
   this.eventParticipantsService.AddNewParticipant(participant).subscribe({
        next:(res:any)=>{
          alert("added Successfully");
          this.click();
          this.closeButton.nativeElement.click();
          console.log("added successgully");
        },
        error:err=>{
          alert(err.error.msg);
          console.log(err);
        }
      });
     

      }
      else{
          this.eventParticipantsService.updateParticipant(participant).subscribe({
        next:(res:any)=>{
          alert("updated successfully");
             this.click();
          this.closeButton.nativeElement.click();
         
        },
        error:err=>{
          alert("You Cant Update this participant because it is already exist");
         
          console.log("");
        }
      });
      }
   
      // Call your API or service here
    }
    
    else {
      console.log('Form is invalid');
    }
  }

    AllParticipants :Filter[]=[];
  AllEvents:Filter[]=[];

    allAtendee(){
      this.AllParticipants=[{Id:null,Name:'Choose Participant'},];

      this.participantServ.getByCriteria(null)?.subscribe({
        next:(res:any)=>{
          res.forEach((x:any)=>{
            let attendee:Filter={
              Id:x.id,
              Name:x.firstName+x.lastName
            }
            this.AllParticipants.push(attendee);
          })
          console.log(this.AllParticipants);
        },
        error:err=>{
          console.log(err.error);
        }




      });
    }

   LoadallEvents(){
       this.AllEvents=[{Id:null,Name:'Choose Event'},];

      this.eventServ.getAllEvents(null)?.subscribe({
        next:(res:any)=>{
          res.forEach((x:any)=>{
            let attendee:Filter={
              Id:x.id,
              Name:x.title
            }
            this.AllEvents.push(attendee);
          })
          console.log(this.AllParticipants);
        },
        error:err=>{
          console.log(err.error);
        }




      });


  }


  LoadParticipant(id:number){
    this.eventParticipantsService.GetParticipant(id).subscribe({
      

      next:(res:any)=>{
        this.addAndUpdateParticipant.patchValue({
          Id:res.id,
            participantId:res.attendeeId,
    eventId:res.eventId
          
        });

        console.log(this.addAndUpdateParticipant);


      },

      error:err=>{
        console.log(err.error.value);
      }
    });


  }
  loadListAndGetParticipant(id:number){
    this.addAndUpdateParticipant.reset();
    this.loadAddListsAndUpdate();
    this.LoadParticipant(id);



  }

  deleteConfirmation :boolean=false;
  title:string="Participant delete confirmation ";
  content:string="Are you sure you want to delete this participant";
  participantId?:number|null=0;

  deleteParticipant(id?:number){
    this.participantId=id;
    this.deleteConfirmation=true;
  }


  confirmAttendeeDelete(confirm:boolean){
    if(confirm){
      this.eventParticipantsService.DeleteParticipant(this.participantId).subscribe({
        next:(res:any)=>{
          alert("Request deleted successfully");
          this.click();
          this.deleteConfirmation = false;
        },
        error:err=>{
          alert("Sorry request cant be deleted");
          this.deleteConfirmation = false;
        }
      });

     
    }
    else{
      this.deleteConfirmation=false;
    }

  }


   RoleCheck(){
    let role=localStorage.getItem("Role")?.toUpperCase();

    return role=="admin".toUpperCase()|| role=="organizer".toUpperCase();
  }




}

