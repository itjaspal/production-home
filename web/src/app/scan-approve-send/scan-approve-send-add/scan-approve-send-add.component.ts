import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { ScanApproveAddView, ScanApproveView, ScanApproveFinView } from '../../_model/scan-approve-send';
import { AuthenticationService } from '../../_service/authentication.service';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { MessageService } from '../../_service/message.service';
import { ScanApproveSendService } from '../../_service/scan-approve-send.service';

@Component({
  selector: 'app-scan-approve-send-add',
  templateUrl: './scan-approve-send-add.component.html',
  styleUrls: ['./scan-approve-send-add.component.scss']
})
export class ScanApproveSendAddComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: ScanApproveAddView,
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

  public validationForm: FormGroup;
  public user: any;
  public model: ScanApproveView = new ScanApproveView();
  public searchModel: ScanApproveAddView = new ScanApproveAddView();

  // public data: any = {};
  public model_scan: ScanApproveFinView = new ScanApproveFinView();  
  public datas: any = {};
  public total = 0;

  
  ngOnInit() {
    this.buildForm();
    this.user = this._authSvc.getLoginUser();
  }

  private buildForm() {
    this.validationForm = this._fb.group({
      set_no: [null, [Validators.required]],
    });
  }

  async onQrEntered(_set_no: string) {

    if (_set_no == null || _set_no == "") {
      return;
    }

    var datePipe = new DatePipe("en-US");

    this.searchModel.user_id = this.user.username;
    this.searchModel.build_type = this.user.branch.entity_code;
    this.searchModel.fin_date = datePipe.transform(this.data.fin_date, 'dd/MM/yyyy').toString();
    this.searchModel.doc_no = this.data.doc_no;
    this.searchModel.wc_code = this.data.wc_code;
    
    console.log(this.searchModel);
    
    this.datas = await this._scanapvSendSvc.ScanApproveSendAdd(this.searchModel);
    this.add(this.datas);
    this.searchModel.set_no = "";

  }

  async save() {

    //console.log(this.data);
    var datePipe = new DatePipe("en-US");

    this.searchModel.user_id = this.user.username;
    this.searchModel.build_type = this.user.branch.entity_code;
    this.searchModel.fin_date = datePipe.transform(this.data.fin_date, 'dd/MM/yyyy').toString();
    this.searchModel.doc_no = this.data.doc_no;
    this.searchModel.wc_code = this.data.wc_code;
    
    console.log(this.searchModel);
    
    this.datas = await this._scanapvSendSvc.ScanApproveSendAdd(this.searchModel);
    this.add(this.datas);
    this.searchModel.set_no = "";


  }

  add(datas: any) {

    let newProd: ScanApproveView = new ScanApproveView();
    newProd.set_no = datas.set_no;
    newProd.prod_code = datas.prod_code;
    newProd.prod_name = datas.prod_name;
    newProd.qty = datas.qty;

    this.total = this.total + datas.qty;

    this.model_scan.datas.push(newProd);

  }
  
  close()
    {
      this.dialogRef.close([]);
    }

}
