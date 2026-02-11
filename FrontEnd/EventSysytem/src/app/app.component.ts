import { Component } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { RequestToBeOrganizerService } from './Services/request-to-be-organizer';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet,RouterLink,RouterLinkActive],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})

 
export class AppComponent {
  title = 'EventSysytem';
   
constructor(public router :Router,private requestToBeAnOrganizer :RequestToBeOrganizerService){}
    showNavbar():boolean{
   
  return this.router.url !== '/login' && this.router.url !== '/sign-up';


 


  }
  resetLocalStorage(){
    localStorage.clear();
  }



     roleAdmincheck(){
    if(localStorage.getItem("Role")?.toUpperCase()==="admin".toUpperCase() ){
      return true;
    }
    else{
      return false;
    }
   }


       rolecAdminAndOrganizerCheck(){
    if(localStorage.getItem("Role")?.toUpperCase()==="admin".toUpperCase() || localStorage.getItem("Role")?.toUpperCase()==="organizer".toUpperCase() ){
      return true;
    }
    else{
      return false;
    }
   }


  
   


   sendRequestToBeAnOrganizer(){
    this.requestToBeAnOrganizer.CreateRequest().subscribe({

      next:(res:any)=>{
        alert("The Request is sended it successfully");
      },
      error:err=>{
        alert(err.error.msg);
        console.log(err);
        console.log(err.error);
        
      }
    });


   }

      

}
//hello world
//ryhj56uj57i