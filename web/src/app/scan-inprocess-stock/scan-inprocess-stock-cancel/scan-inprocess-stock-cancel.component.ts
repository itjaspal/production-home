import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { ScanInprocessSearchView, JobInProcessStockView, JobInProcessStockSearchView, JobInProcessStockScanFinView, JobInProcessStockScanView } from '../../_model/job-inprocess-stock';
import { AuthenticationService } from '../../_service/authentication.service';
import { MessageService } from '../../_service/message.service';
import { ScanInprocessStockService } from '../../_service/scan-inprocess-stock.service';
import { SubProductCancelSearchComponent } from '../sub-product-cancel-search/sub-product-cancel-search.component';

@Component({
  selector: 'app-scan-inprocess-stock-cancel',
  templateUrl: './scan-inprocess-stock-cancel.component.html',
  styleUrls: ['./scan-inprocess-stock-cancel.component.scss']
})
export class ScanInprocessStockCancelComponent implements OnInit {

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
    private cdr: ChangeDetectorRef,

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

  // @ViewChild('qr') qrElement:ElementRef;


  ngOnInit() {
    this.buildForm();
    this.user = this._authSvc.getLoginUser();
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
    console.log(this.searchModel);
  }

  private buildForm() {
    this.validationForm = this._fb.group({
      sub_prod_code: [null, [Validators.required]],
      qty: [null, []]
    });
  }


  async save() {

    if (this.searchModel.qty == 0) {
      this._msgSvc.warningPopup("ต้องใส่จำนวนยกเลิก");
    }
    else {
      var datePipe = new DatePipe("en-US");
      this.searchModel.entity = this.data.entity;
      this.searchModel.wc_code = this.data.wc_code;
      this.searchModel.req_date = datePipe.transform(this.data.req_date, 'dd/MM/yyyy').toString();
      this.searchModel.por_no = this.data.por_no;
      this.searchModel.ref_no = this.data.ref_no;
      this.searchModel.user_id = this.user.username;
      this.searchModel.build_type = this.user.branch.entity_code;
      

      this.datas = await this._jobInprocessSvc.searchCancel(this.searchModel);
      console.log(this.datas);
      this.add(this.datas);
      this.searchModel.sub_prod_code = "";
      this.searchModel.qty = 0;
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

  }


  openSearchProductModal() {

    const dialogRef = this._dialog.open(SubProductCancelSearchComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '80%',
      width: '80%',
      data: {
        entity: this.data.entity,
        req_date: this.searchModel.req_date,
        wc_code: this.data.wc_code,
        por_no: this.searchModel.por_no,
        ref_no: this.searchModel.ref_no,
      }

    });

    dialogRef.afterClosed().subscribe(result => {
      if (result.length > 0) {
        this.add_prod(result);
      }
    })
  }

  add_prod(datas: any) {

    console.log(datas);
    this.searchModel.sub_prod_code = datas[0].sub_prod_code;
    this.searchModel.prod_code = datas[0].prod_code;
    this.searchModel.qty = 0;
  }



  close() {
    this.dialogRef.close();
  }

}
