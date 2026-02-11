import { Component, ElementRef, ViewChild } from '@angular/core';
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
  @ViewChild('closeAddModal') closeAddModal!:ElementRef;
  @ViewChild('closeEditModal')closeEditModal!:ElementRef;

  deleteConfirmation:boolean=false;
  showDeleteConfirmation(Id?:number|null){
    this.AttendeeIdToBeDeleted=Id??null;
    this.deleteConfirmation=true;
  }
  title="Delete Attendee Confirmation";
  content="Are you sure you want to delete this Attendee?"


  apiErrors: string[] = [];

  AttendeeAddForm:FormGroup=new FormGroup({
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


    AttendeeEditForm:FormGroup=new FormGroup({
    userName: new FormControl<string>("",Validators.required),
    password:new FormControl<string|null>(null),
    id:new FormControl<number|null>(null),
      firstName:new FormControl<string>("",Validators.required),
      lastName:new FormControl<string>("",Validators.required),
        email:new FormControl<string>("",Validators.required),
         userId:new FormControl<number|null>(null),
         gender:new FormControl<boolean>(false,Validators.required),
           roleId:new FormControl<number|null>(null),
            isActive:new FormControl<boolean>(false),

      
   

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

    

    if(this.AttendeeEditForm.value.id==null){
             let addAttendee :Attendee={
      userName:this.AttendeeAddForm.value.userName,
      password:this.AttendeeAddForm.value.password,
      Id:this.AttendeeAddForm.value.id??null,
      userId:0,
      firstName:this.AttendeeAddForm.value.firstName,
      lastName:this.AttendeeAddForm.value.lastName,
      email:this.AttendeeAddForm.value.email,
      gender:this.AttendeeAddForm.value.gender,
      Role:this.AttendeeAddForm.value.roleId
      

    };
       
 
    this._attendeeService.AddAtendee(addAttendee).subscribe({

      next:(res:any)=>{
        console.log("Successfully added");
        this.AttendeeAddForm.reset();
        this.closeAddModal?.nativeElement.click();
        this.click();
      },
      error:(err)=>{
        let errorr:any[]=err.error?.errors??[];
        console.log(err.error);
        console.error("error");
       
        alert(err.error.fullName+"\n"+err.error.userName+"\n"+err.error.email);
      
       
        

      
        
      
      
   
   
      }
    });

    }
    else{
             let editAttendee :Attendee={
      userName:this.AttendeeEditForm.value.userName,
      password:this.AttendeeEditForm.value.password,
      Id:this.AttendeeEditForm.value.id,
      userId:0,
      firstName:this.AttendeeEditForm.value.firstName,
      lastName:this.AttendeeEditForm.value.lastName,
      email:this.AttendeeEditForm.value.email,
      gender:this.AttendeeEditForm.value.gender,
      Role:this.AttendeeEditForm.value.roleId,
      isActive:this.AttendeeEditForm.value.isActive
      

    };
      this._attendeeService.EditAtendee(editAttendee).subscribe({
          next:(res:any)=>{
        console.log("Successfully updated");
        this.AttendeeEditForm.reset();
        this.click();
        this.closeEditModal.nativeElement.click();
      },
      error:err=>{
   console.log(err.error);
        console.error("error");
       
        alert(err.error.fullName+"\n"+err.error.userName+"\n"+err.error.email);


      }

      });
   
      
    }
  
   
  }
  ClearForm(){
    this.AttendeeEditForm.reset();
    this.AttendeeAddForm.reset();
  }

  loadAttendeeForm(id?:number|null){
     if(!id){
      return;
     }
      this._attendeeService.getAttendeeById(id).subscribe({
        next:(res:any)=>{
          this.AttendeeEditForm.patchValue({
            userName:res.userName,
            id:res.id,
            firstName:res.firstName,
            lastName:res.lastName,
            email:res.email,
            userId:res.userId,
            gender:res.gender,
            roleId:res.role,
            password:null,
            isActive:res.isActive

            
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
           this.click();
          this.deleteConfirmation=false;
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

   roleCheck(para:string):boolean{
 let role=localStorage.getItem("Role")?.toUpperCase();
 para=para.toUpperCase();
  return role==para.toUpperCase();
 }

    RoleCheck(){
 let role=localStorage.getItem("Role")?.toUpperCase();
 
  return role=="admin".toUpperCase();
 }

}
