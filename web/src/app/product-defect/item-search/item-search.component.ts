import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ItemNoSearchView } from '../../_model/product-defect';
import { ProductDefectService } from '../../_service/product-defect.service';

@Component({
  selector: 'app-item-search',
  templateUrl: './item-search.component.html',
  styleUrls: ['./item-search.component.scss']
})
export class ItemSearchComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: ItemNoSearchView,
    private _fb: FormBuilder,
    private _defectSvc: ProductDefectService,
    private cdr: ChangeDetectorRef,
  ) { }

  // public validationForm: FormGroup;
  user:any = {};
  // public model : ProductSearchView  = new ProductSearchView();
  //datas: any;
  datas: any = {};
 

  async ngOnInit() {
    //console.log(this.data);
    this.datas = await this._defectSvc.getItemList(this.data.entity,this.data.por_no);
    console.log(this.datas);
    //this.search();
        
  }


  add()
  { 
    let addList: any = this.datas.datas.filter(x => x.selected);
    //console.log(addList);

    this.dialogRef.close(addList);
  }


  close()
  {
    this.dialogRef.close([]);
  }

}
