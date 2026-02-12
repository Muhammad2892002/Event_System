import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators, ReactiveFormsModule, ɵInternalFormsSharedModule } from '@angular/forms';
import { Router, RouterLink } from "@angular/router";
import { AttendeeService } from '../../Services/AttendeeService';
import { Attendee } from '../../Interface/attendee';

@Component({
  selector: 'app-sign-up',
  imports: [RouterLink, ɵInternalFormsSharedModule,ReactiveFormsModule],
  templateUrl: './sign-up.html',
  styleUrl: './sign-up.css',
})
export class SignUp {

  constructor(private attendeeServ:AttendeeService,private router:Router){}




     AttendeeForm:FormGroup=new FormGroup({
    userName: new FormControl<string>("",Validators.required),
    password:new FormControl<string|null>("",Validators.required),
    id:new FormControl<number|null>(null),
      firstName:new FormControl<string>("",Validators.required),
      lastName:new FormControl<string>("",Validators.required),
        email:new FormControl<string>("",Validators.required),
         userId:new FormControl<number|null>(null),
         gender:new FormControl<boolean|null>(null,Validators.required),
           roleId:new FormControl<number|null>(null),
            isActive:new FormControl<boolean>(false),

      
   

  });


  showForms(){
    console.log(this.AttendeeForm.value);
    alert(this.AttendeeForm.value);
  }

  addAttendee(){
    let attendee:Attendee={
      userName:this.AttendeeForm.value.userName,
      password:this.AttendeeForm.value.password,
      firstName:this.AttendeeForm.value.firstName,
      lastName:this.AttendeeForm.value.lastName,
      email:this.AttendeeForm.value.email,
      gender:this.AttendeeForm.value.gender,
      


   

    };
    this.attendeeServ.AddAtendee(attendee).subscribe({

      next:(res:any)=>{
        alert("Account created successfully");
        this.router.navigate(['/login']);
      
       


      },
      error:err=>{
        alert(err.error.fullName+"\n"+err.error.userName+"\n"+err.error.email);
        console.log(err.error);
        alert("");
      }
    });
   
  }

}
