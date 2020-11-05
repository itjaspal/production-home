import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { SendProductSearch } from '../../_model/scan-receive';
import { AuthenticationService } from '../../_service/authentication.service';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { MessageService } from '../../_service/message.service';
import { ScanReceiveService } from '../../_service/scan-receive.service';

@Component({
  selector: 'app-scan-receive-product-view',
  templateUrl: './scan-receive-product-view.component.html',
  styleUrls: ['./scan-receive-product-view.component.scss']
})
export class ScanReceiveProductViewComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: SendProductSearch,
    private _fb: FormBuilder,
    private _authSvc: AuthenticationService,
    private _dialog: MatDialog,
    private _msgSvc: MessageService,
    private _router: Router,
    private _actRoute: ActivatedRoute,
    private _scanRecSvc: ScanReceiveService,
    private cdr: ChangeDetectorRef,
    private _dll: DropdownlistService,) { }


  public datas: any = {};

  async ngOnInit() {
    console.log(this.data.doc_no);
    console.log(this.data.entity);

    this.datas =  await this._scanRecSvc.getProductDetail(this.data.entity,this.data.doc_no);

    console.log(this.datas);
  }

  close() { 
    this.dialogRef.close();
  }

}
