import { MatDialog } from '@angular/material';
import { LoaderService } from './loader.service';
import { Injectable } from '@angular/core';
import {
    HttpInterceptor,
    HttpRequest,
    HttpResponse,
    HttpHandler,
    HttpEvent,
    HttpErrorResponse
} from '@angular/common/http';

import { Observable, throwError } from 'rxjs';
import { map, catchError, finalize } from 'rxjs/operators';
import { MessageService } from './message.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

    constructor(
        public _loaderSvc: LoaderService,
        public messageService: MessageService,
    ) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // console.log("loader --start");
        this._loaderSvc.display(true);

        request = request.clone({ headers: request.headers.set('token', '1') });
        // alert(JSON.stringify(request))
        return next.handle(request).pipe(
            map((event: HttpEvent<any>) => {
                if (event instanceof HttpResponse) {
                    // console.log('event--->>>', event);
                    // this.errorDialogService.openDialog(event);
                }
                return event;
            }),
            catchError((error: HttpErrorResponse) => {
                console.log(error);
                // let data = {};
                // data = {
                //     reason: error && error.message ? error.message : '',
                //     status: error.status
                // };
                // this.errorDialogService.openDialog(data);
                // alert(JSON.stringify(error))
                this.messageService.errorPopup(error.error.Message);
                // console.log(data);
                return throwError(error);
            }),
            finalize(() => {
                // console.log("loader --end");
                this._loaderSvc.display(false);
            })
        );
        // return next.handle(request)
        //     .catch((error: any) => {
        //         if (error instanceof HttpErrorResponse) {
        //             console.log(error);

        //             // this._dialog.open(AlertDialogComponent, {
        //             //     data: {
        //             //         type: "error",
        //             //         title: error.message,
        //             //         msg: (typeof error.error === 'string') ? error.error : ''
        //             //     }
        //             // });
        //             alert(error.message);

        //         } else {
        //             return Observable.throw(error);
        //         }
        //     })
        //     .finally(() => {
        //         // console.log("loader --end");
        //         this._loaderSvc.display(false);
        //     });
    }
}