import { ConfirmMessageComponent } from './../modal/confirm-message/confirm-message.component';
import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material';
import { PopupMessageComponent } from '../modal/message/popup-message.component';

@Injectable()
export class MessageService {

    constructor(
        protected dialog: MatDialog
    ) {

    }

    async successPopup(message: string) {
        setTimeout(() => this.dialog.open(PopupMessageComponent, {
            maxWidth: '1000px',
            minWidth: '300px',
            autoFocus: false,
            data: { info: true, message: message }
        }));
    }

    async errorPopup(message: string) {
        setTimeout(() => this.dialog.open(PopupMessageComponent, {
            maxWidth: '1000px',
            minWidth: '300px',
            autoFocus: false,
            data: { error: true, message: message }
        }));
    }

    async warningPopup(message: string) {
        setTimeout(() => this.dialog.open(PopupMessageComponent, {
            maxWidth: '1000px',
            minWidth: '300px',
            autoFocus: false,
            data: { warning: true, message: message }
        }));
    }

    confirmPopup(message: string, callBack) {
        let dialogRef = this.dialog.open(ConfirmMessageComponent, {
            maxWidth: '1000px',
            autoFocus: false,
            data: { message: message }
        });

        dialogRef.afterClosed().subscribe(confirm => {
            callBack(confirm);
        });
    }

}
