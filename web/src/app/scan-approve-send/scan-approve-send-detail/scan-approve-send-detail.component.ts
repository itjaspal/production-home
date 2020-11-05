import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { SendApproveProductSearch } from '../../_model/scan-approve-send';
import { SendProductSearch } from '../../_model/scan-receive';
import { AuthenticationService } from '../../_service/authentication.service';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { MessageService } from '../../_service/message.service';
import { ScanApproveSendService } from '../../_service/scan-approve-send.service';

@Component({
  selector: 'app-scan-approve-send-detail',
  templateUrl: './scan-approve-send-detail.component.html',
  styleUrls: ['./scan-approve-send-detail.component.scss']
})
export class ScanApproveSendDetailComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: SendApproveProductSearch,
    private _fb: FormBuilder,
    private _authSvc: AuthenticationService,
    private _dialog: MatDialog,
    private _msgSvc: MessageService,
    private _router: Router,
    private _actRoute: ActivatedRoute,
    private _scanapvSendSvc: ScanApproveSendService,
    private cdr: ChangeDetectorRef,
    private _dll: DropdownlistService,
  ) { }

  public datas: any = {};

  async ngOnInit() {
    console.log(this.data.doc_no);
    console.log(this.data.entity);

    this.datas =  await this._scanapvSendSvc.getProductDetail(this.data.entity,this.data.doc_no,this.data.wc_code);

    console.log(this.datas);
  }

  close() { 
    this.dialogRef.close();
  }

}
