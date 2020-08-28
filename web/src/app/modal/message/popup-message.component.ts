
import { Inject, Component, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";

@Component({
  selector: 'app-popup-message',
  templateUrl: 'popup-message.component.html',
  styleUrls: [
    'popup-message.component.scss'
  ]
})
export class PopupMessageComponent implements OnInit {


  constructor(
    public dialogRef: MatDialogRef<PopupMessageComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {

  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
