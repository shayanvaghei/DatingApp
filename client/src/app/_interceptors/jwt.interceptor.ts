import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/user';
import { take } from 'rxjs/operators';

// interceptor to send the token

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService: AccountService) {}
 

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
     // in order to get the token we need to get the current user outside of the observable
    let currentUser: User;
    // by take, we say we want to complete after we receive one of these current user
    // and this way we don't need to unsubscribe because once an observable has completed then we are effectivly not subscribed
    // to it any more. So whenever we are not sure whether we are going to unsubscribe things from somehting then what we can do is just
    // simply add that pipe and take one in this case and then we go and subscribe. We kind of guarantee that we unsubscribe form that
    // our Interceptors are going to be initialized after we start our application because of that part of app module we added to the providers
    // and there always be going to be around until we close our application. So weathear 
    // we will use this technique to continueousely ensure that anywhere else we use this we do take care of compliting the subscrition
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => currentUser = user);
    if (currentUser) {
      // we want to clone our request and add our authentication headero to this
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${currentUser.token}`
        }
      })
    }

    // since we cloned our request, once we return from this its that request when we logged in is going receive our Authorization header and send
    // this with our request
    return next.handle(request);
  }
}
