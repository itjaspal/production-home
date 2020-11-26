import { scan } from 'rxjs/operators';
import { DatePipe } from '@angular/common';
import { Component, ElementRef, Inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { JobInProcessScanView } from '../../_model/job-inprocess';
import { JobInProcessStockView, JobInProcessStockSearchView, JobInProcessStockScanFinView, JobInProcessStockScanView, ScanInprocessSearchView } from '../../_model/job-inprocess-stock';
import { AuthenticationService } from '../../_service/authentication.service';
import { MessageService } from '../../_service/message.service';
import { ScanInprocessStockService } from '../../_service/scan-inprocess-stock.service';

@Component({
  selector: 'app-scan-inprocess-stock-scan-add',
  templateUrl: './scan-inprocess-stock-scan-add.component.html',
  styleUrls: ['./scan-inprocess-stock-scan-add.component.scss']
})
export class ScanInprocessStockScanAddComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: ScanInprocessSearchView,
    private _fb: FormBuilder,
    private _authSvc: AuthenticationService,
    private _dialog: MatDialog,
    private _msgSvc: MessageService,
    private _router: Router,
    private _actRoute: ActivatedRoute,
    private _jobInprocessSvc: ScanInprocessStockService,
    //private snackBar: MatSnackBar
  ) { }

  public validationForm: FormGroup;
  public user: any;
  public model: JobInProcessStockView = new JobInProcessStockView();
  public searchModel: JobInProcessStockSearchView = new JobInProcessStockSearchView();

  // public data: any = {};
  public model_scan: JobInProcessStockScanFinView = new JobInProcessStockScanFinView();

  public datas: any = {};
  public count = 0;
  public current_date = "";
  public current_time = "";

  // public result: any = {};

  @ViewChild('qr') qrElement: ElementRef;

  ngAfterViewInit() {
    //this.qrElement.nativeElement.focus();
  }

  ngOnInit() {
    this.buildForm();
    this.user = this._authSvc.getLoginUser();
    var datePipe = new DatePipe("en-US");

    this.current_date = datePipe.transform(new Date(), 'dd/MM/yyyy').toString();
    this.current_time = datePipe.transform(new Date(), 'HH:mm').toString();

    this.searchModel.entity = this.data.entity;
    this.searchModel.wc_code = this.data.wc_code;
    this.searchModel.req_date = datePipe.transform(this.data.req_date, 'dd/MM/yyyy').toString();
    this.searchModel.por_no = this.data.por_no;
    this.searchModel.ref_no = this.data.ref_no;
    this.searchModel.user_id = this.user.username;
    this.searchModel.build_type = this.user.branch.entity_code;
    console.log(this.data);
  }

  private buildForm() {
    this.validationForm = this._fb.group({
      qr: [null, []],
      qty: [null, []]
    });
  }

  async onQrEntered(_qr: string) {

    if (_qr == null || _qr == "") {
      return;
    }
    if(this.searchModel.qty == 0)
    {
      this._msgSvc.warningPopup("ต้องใส่จำนวน");
    }
    else
    {

    var datePipe = new DatePipe("en-US");

    //  this.searchModel.entity = this.data.entity;
    //  this.searchModel.wc_code = this._actRoute.snapshot.params.wc_code;
    this.searchModel.scan_data = _qr;
    //  this.searchModel.req_date  = datePipe.transform(this.data.req_date, 'dd/MM/yyyy').toString();

    //  this.searchModel.user_id = this.user.username;
    //  this.searchModel.build_type = this.user.branch.entity_code;



    this.datas = await this._jobInprocessSvc.searchScanAdd(this.searchModel);
    console.log(this.datas);
    this.qrElement.nativeElement.focus();


    this.add(this.datas);
    }

  }

  add(datas: any) {

    let newProd: JobInProcessStockScanView = new JobInProcessStockScanView();
    newProd.prod_code = datas.prod_code;
    newProd.sub_prod_code = datas.sub_prod_code;
    newProd.sub_prod_name = datas.sub_prod_name;
    newProd.por_no = datas.por_no;
    newProd.ref_no = datas.ref_no;
    newProd.entity = datas.entity;
    newProd.qty = datas.qty;
    newProd.wc_code = datas.wc_code;

    this.model_scan.datas.push(newProd);
    console.log(this.model_scan);
    // // Group By Product
    // var result = [];
    // this.model_scan.datas.forEach(function (a) {
    //   if ( !this[a.prod_code] && !this[a.prod_name] ) {
    //       this[a.prod_code] = { prod_code: a.prod_code, prod_name: a.prod_name, qty: 0 };
    //       result.push(this[a.prod_code]);
    //   } 
    //   this[a.prod_code].qty += a.qty;

    // }, Object.create(null));

    // this.model_scan.datas = result;
    // //console.log(result)
    // console.log(this.model_scan.datas);


  }

  cancel(_index, scan) {
    console.log(scan);

    this._msgSvc.confirmPopup("ยืนยันยกเลิกบันทึกรับมอบ", async result => {
      if (result) {
        let res: any = await this._jobInprocessSvc.Cancel(scan);

        this._msgSvc.successPopup(res.message);
        this.model_scan.datas.splice(_index, 1);

      }
    })
  }


  close() {
    this.dialogRef.close();
  }

}
