import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  let token=localStorage.getItem("Token");
  if(token!=null){
    let authReq=req.clone({
      headers:req.headers.set("Authorization",`Bearer ${token}`)
    });
    return next(authReq);
  }
  return next(req);
};
