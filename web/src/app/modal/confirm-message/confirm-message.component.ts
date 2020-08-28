import { Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-confirm-message',
  templateUrl: './confirm-message.component.html',
  styles: []
})
export class ConfirmMessageComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<ConfirmMessageComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {
  }

  onYesClick(){
    this.dialogRef.close(true);
  }

  onNoClick(){
    this.dialogRef.close(false);
  }

}
