import { PrintInprocessDetailComponent } from './../../print-inprocess-tag/print-inprocess-detail/print-inprocess-detail.component';
import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { ProductSearchComponent } from '../../scan-inprocess/product-search/product-search.component';
import { JobInProcessScanView } from '../../_model/job-inprocess';
import { ScanSendView, ScanSendSearchView, ScanSendFinView, ScanSendFinDataView } from '../../_model/scan-send';
import { AuthenticationService } from '../../_service/authentication.service';
import { MessageService } from '../../_service/message.service';
import { ScanSendService } from '../../_service/scan-send.service';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import * as moment from 'moment';


@Component({
  selector: 'app-scan-send',
  templateUrl: './scan-send.component.html',
  styleUrls: ['./scan-send.component.scss']
})
export class ScanSendComponent implements OnInit {

  @ViewChild('req_date') req_date: ElementRef;
  
  constructor(
    private _fb: FormBuilder,
    private _authSvc: AuthenticationService,
    private _dialog: MatDialog,
    private _msgSvc: MessageService,
    private _router: Router,
    private _actRoute: ActivatedRoute,
    private _scanSendSvc: ScanSendService,
    private cdr: ChangeDetectorRef,
    private _dll: DropdownlistService,
  ) { }

  public validationForm: FormGroup;
  public user: any;
  public model: ScanSendView = new ScanSendView();
  public searchModel: ScanSendSearchView = new ScanSendSearchView();

  public data: any = {};
  public model_scan: ScanSendFinView = new ScanSendFinView();

  public datas: any = {};
  public datas_print: any = {};
  public wclist: any; 

  public data_qty : any;
  
  async ngOnInit() {
    this.buildForm();
    this.user = this._authSvc.getLoginUser();
    this.wclist = await this._dll.getDdlWCSend(this.user.username);
    if(this.wclist.length==1)
    {
      this.searchModel.wc_code = this.wclist[0].key;
    }
    //this.searchModel.req_date = new Date()
    console.log(this.data);
    
  }

  private buildForm() {
    this.validationForm = this._fb.group({
      pcs_barcode: [null, [Validators.required]],
      wc_code: [null, [Validators.required]],
      req_date: [null, [Validators.required]],
      set_no:[null, []]
    });
  }

  async save() {

    var datePipe = new DatePipe("en-US");
    this.searchModel.req_date = datePipe.transform(this.searchModel.req_date, 'dd/MM/yyyy').toString();
    this.searchModel.user_id = this.user.username;
    console.log(this.searchModel);
    
    this.datas = await this._scanSendSvc.scansendadd(this.searchModel);

    console.log(this.datas);

    this.add(this.datas);
    this.searchModel.pcs_barcode = "";
    this.model.show_qty = this.datas.scan_qty + " / " + this.datas.set_qty;
    this.searchModel.req_date = this.datas.req_date; 
    
    if(this.datas.set_qty == this.datas.scan_qty)
    {
    
      this.datas_print = await this._scanSendSvc.PrintSticker(this.datas);
      this.model.show_qty = "";
    }



  }

  add(datas: any) {

    let newProd: ScanSendFinDataView = new ScanSendFinDataView();
    newProd.prod_code = datas.prod_code;
    newProd.prod_name = datas.prod_name;
    newProd.pcs_barcode = datas.pcs_barcode;
    newProd.job_no = datas.job_no;
    newProd.wc_code = datas.wc_code;
    newProd.req_date = datas.req_date;

    this.model_scan.datas.push(newProd);  
    console.log(this.model_scan.datas);

  }

  async print(){
    this.datas.set_qty = this.datas.scan_qty;
    this.datas_print = await this._scanSendSvc.PrintSticker(this.datas);
    this.model.show_qty = "";
  }

  openSearchSetNoModal(SaleTransactionItemView = null)
  {
    const dialogRef = this._dialog.open(ProductSearchComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '80%',
      width: '80%',
      data: {
        req_date: this._actRoute.snapshot.params.req_date,
        wc_code:this._actRoute.snapshot.params.wc_code,
        pdjit_grp:this._actRoute.snapshot.params.pdjit_grp
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
    // this.searchModel.bar_code = datas[0].bar_code;
    // this.searchModel.qty = 1;
  }  


  async delete(_index,scan) {
    console.log(scan); 

    this._msgSvc.confirmPopup("ยืนยันยกเลิกบันทึกส่งมอบ", async result => {
      if (result) {
        let res: any = await this._scanSendSvc.delete(scan);

        this._msgSvc.successPopup(res.message);

        this.data_qty = await this._scanSendSvc.getscanqty(scan);

        console.log(this.data_qty);
        this.model.show_qty = this.data_qty.scan_qty + " /" + this.data_qty.set_qty;
        this.model_scan.datas.splice(_index, 1);

      }
    })

  }


  close() {
    this._router.navigateByUrl('/app/home');
  }


}
