import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgClass } from '@angular/common';
import { AttendeeService } from '../../Services/AttendeeService';
import { Attendee } from '../../Interface/attendee';
import { NgxPaginationModule } from 'ngx-pagination';
import { DeleteConfirmation } from '../delete-confirmation/delete-confirmation';

@Component({
  selector: 'app-attendee',
  imports: [ReactiveFormsModule, NgClass, NgxPaginationModule,DeleteConfirmation],
  templateUrl: './attendee.html',
  styleUrl: './attendee.css',
})
export class AttendeeComponent {
  constructor(private _attendeeService: AttendeeService) { }

  searchForm: FormGroup = new FormGroup({
    FirstName: new FormControl<string | null>(null),
    LastName: new FormControl<string | null>(null),
    RoleId: new FormControl<number | null>(null),
    GenderId: new FormControl<boolean | null>(null),
    ISActivated: new FormControl<boolean | null>(null),


  });
  errorFlag:boolean=false;

  deleteConfirmation:boolean=false;
  showDeleteConfirmation(Id?:number|null){
    this.AttendeeIdToBeDeleted=Id??null;
    this.deleteConfirmation=true;
  }
  title="Delete Attendee Confirmation";
  content="Are you sure you want to delete this Attendee?"


  apiErrors: string[] = [];

  AttendeeForm:FormGroup=new FormGroup({
    userName: new FormControl<string>("",Validators.required),
    password:new FormControl<string>("",Validators.required),
    id:new FormControl<number|null>(null),
      firstName:new FormControl<string>("",Validators.required),
      lastName:new FormControl<string>("",Validators.required),
        email:new FormControl<string>("",Validators.required),
         userId:new FormControl<number|null>(null),
         gender:new FormControl<boolean>(false,Validators.required),
           roleId:new FormControl<number|null>(null),

      
   

  });


  paginationConfig={
    itemsPerPage:5,
    currentPage:1,
  }
  ngOnInit(){
    this.click();//load all attendees at the beginning
  }

  changePage(pageNumber:number){
    this.paginationConfig.currentPage=pageNumber;
  }
  attendees: Attendee[] = [];
  click() {
    
    this.attendees = [];
    this._attendeeService.getByCriteria(this.searchForm.value).subscribe({
      next: (element: any) => {
        element.forEach((x:any)=>{
           let attendee: Attendee = {
              userName: x.userName,
              Id: x.id,
              
              firstName: x.firstName,
              lastName: x.lastName,
              email: x.email,
              userId: x.userId,
              gender: x.gender,
              genderName: x.genderName,
              Role: x.role,
              RoleName: x.roleName,
              isActive: x.isActive,
              password: null,



            }
            this.attendees.push(attendee);
        })
        



       },

      error: err => {
        this.errorFlag=true;
          this.apiErrors = err.error; 
        console.log(err.error?.message ?? err?.message ?? "Http Response Error");
          console.log('Errors:', err.error); 
          console.log(this.apiErrors.values);
  
  



      }



    });
    console.log(this.attendees);




  }
  AddNewAttendee(){
       let attendee :Attendee={
      userName:this.AttendeeForm.value.userName,
      password:this.AttendeeForm.value.password,
      Id:this.AttendeeForm.value.id??null,
      userId:0,
      firstName:this.AttendeeForm.value.firstName,
      lastName:this.AttendeeForm.value.lastName,
      email:this.AttendeeForm.value.email,
      gender:this.AttendeeForm.value.gender,
      Role:this.AttendeeForm.value.roleId
      

    };

    if(attendee.Id==null){
        console.log(this.AttendeeForm.valid);
 
    this._attendeeService.AddAtendee(attendee).subscribe({

      next:(res:any)=>{
        console.log("Successfully added");
        this.AttendeeForm.reset();
      },
      error:err=>{
                console.log('FULL ERROR:', err);
      console.log('API Validation Errors:', err.error?.errors);
   
      }
    });

    }
    else{
      this._attendeeService.EditAtendee(attendee).subscribe({
          next:(res:any)=>{
        console.log("Successfully updated");
        this.AttendeeForm.reset();
      },
      error:err=>{
           console.log('FULL ERROR:', err);
      console.log('API Validation Errors:', err.error?.errors);


      }

      });
   
      
    }
  
   
  }
  ClearForm(){
    this.AttendeeForm.reset();
  }

  loadAttendeeForm(id?:number|null){
     if(!id){
      return;
     }
      this._attendeeService.getAttendeeById(id).subscribe({
        next:(res:any)=>{
          this.AttendeeForm.patchValue({
            userName:res.userName,
            id:res.id,
            firstName:res.firstName,
            lastName:res.lastName,
            email:res.email,
            userId:res.userId,
            gender:res.gender,
            roleId:res.role,
            password:null

            
          });
        

        },
        error:err=>{
          console.log(err.error.value);
          console.log(err);
        }

      
      });
      
    



  }
  AttendeeIdToBeDeleted:number |null=null;

  confirmAttendeeDelete(confirm:boolean){
    if(confirm){
      this._attendeeService.DeleteAttendee(this.AttendeeIdToBeDeleted).subscribe({


        next:(res:any)=>{
          alert("Deleted successfully");
        },
        error:err=>{
          console.log(err);
          

          alert(err.error.msg);
        }

       
      });
    
    }
    else{
      this.deleteConfirmation=false;

    }


  }

}
