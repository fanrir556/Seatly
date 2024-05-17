<style></style>

<template>
  <section v-if="isMg == true" class="p-0">
    <div class="d-flex justify-content-between mt-1">
      <button class="btn btn-gradient mb-2 me-1" id="btnCreatePS">新增商品</button>
      <button class="btn btn-gradient mb-2 me-1" id="btnSaveEditPS">儲存修改</button>
      <button class="btn btn-gradient mb-2 me-1" id="btnSaveNewPS" style="display: none">
        儲存新增
      </button>
      <button class="btn btn-gradient mb-2 ms-auto" id="btnDeleteSelected">刪除勾選商品</button>
    </div>
    <div id="requiredContent"></div>
    <table class="table table-striped text-center">
      <thead>
        <tr>
          <th v-for="dName in results.dNames" :key="dName">
            {{ dName }}
          </th>
          <!-- <th style="width: 200px">
            @Html.DisplayNameFor(model => model.pointsShopPd.FirstOrDefault().ProductImage)
          </th>
          <th style="width: 120px">
            @Html.DisplayNameFor(model => model.pointsShopPd.FirstOrDefault().ProductName)
          </th>
          <th style="width: 80px">
            @Html.DisplayNameFor(model => model.pointsShopPd.FirstOrDefault().ProductPrice)
          </th>
          <th style="width: 120px">
            @Html.DisplayNameFor(model => model.pointsShopPd.FirstOrDefault().Category)
          </th>
          <th>
            @Html.DisplayNameFor(model => model.pointsShopPd.FirstOrDefault().ProductDescription)
          </th> -->
          <th style="width: 50px">刪除</th>
        </tr>
      </thead>
      <tbody>
        <tr class="align-middle" id="createPS" style="display: none">
          <td></td>
          <td>
            <img
              :src="noImgUrl"
              class="img-fluid rounded"
              style="width: 100px"
              alt="..."
              title=".."
            />
            <input class="form-control p-0 pimg" type="file" accept="image/*" />
          </td>
          <td><input class="form-control p-0 text-center" style="width: 120px" type="text" /></td>
          <td><input class="form-control p-0 text-center" style="width: 80px" type="number" /></td>
          <td>
            <select class="form-control p-0 text-center">
              <option disabled selected>請選擇</option>
              <option v-for="sList in results.sList1" :key="sList"></option>
            </select>
            <input
              class="form-control p-0 text-center"
              style="width: 104px; display: none"
              type="text"
              placeholder="請輸入類別"
            />
            <p class="text-gradient" id="addCategory">新增類別?</p>
          </td>
          <td><textarea class="form-control p-0"></textarea></td>
          <td></td>
        </tr>
        <tr
          v-for="{
            productId,
            productName,
            category,
            productPrice,
            productDescription,
            productImage
          } in results.shops"
          :key="productId"
          class="align-middle"
        >
          <td>productId</td>
          <td>
            <img
              :src="productImage"
              class="img-fluid rounded"
              style="width: 100px"
              :alt="productImage"
              :title="productName"
            />
            <input class="form-control p-0 pimg" type="file" accept="image/*" />
          </td>
          <td>
            <input
              class="form-control p-0 text-center"
              style="width: 120px"
              type="text"
              :value="productName"
            />
          </td>
          <td>
            <input
              class="form-control p-0 text-center"
              style="width: 80px"
              type="number"
              :value="productPrice"
            />
          </td>
          <td>
            <select class="form-control p-0 text-center" :value="category">
              <option v-for="sList in results.sList1" :key="sList"></option>
            </select>
          </td>
          <td>
            <textarea class="form-control p-0">{{ productDescription }}</textarea>
          </td>
          <td><input type="checkbox" class="form-check-input" /></td>
        </tr>
      </tbody>
    </table>
  </section>
  <section v-else class="p-0">
    <div v-if="results.userPoints != null">
      <div class="text-gradient d-inline">您目前持有:</div>
      <div class="d-inline ms-2 me-2" data-userId="@Model.user.Id">
        {{ results.userPoints }}
      </div>
      <div class="text-gradient d-inline">點數</div>
    </div>
    <div
      v-for="{
        productId,
        productName,
        category,
        productPrice,
        productDescription,
        productImage
      } in results.shops"
      :key="productId"
      class="card mb-3"
    >
      <div class="row g-0">
        <div class="col-md-4">
          <img
            :src="productImage"
            class="img-fluid rounded-start"
            style="width: 300px"
            :alt="productName"
            :title="productName"
          />
        </div>
        <div class="col-md-8">
          <div class="card-body">
            <span class="position-absolute end-0 me-2">{{ category }}</span>
            <h5 class="card-title text-gradient fs-3">{{ productName }}</h5>
            <p class="card-text">{{ productDescription }}</p>
            <p class="card-text text-gradient">兌換需要 {{ productPrice }} 點數</p>
            <button
              class="btn btn-gradient position-absolute bottom-0 end-0 mb-1 me-1 btn-exchange"
              data-bs-toggle="modal"
              data-bs-target="#exchangeModal"
              @click="pdExchange(productId, productName, productPrice)"
            >
              兌換
            </button>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup>
import { ref, onMounted, reactive } from 'vue'

var noImgUrl = ref(import.meta.env.VITE_API_SeatlyUrl + '/images/NoImage.jpg')
const props = defineProps({
  isMg: Boolean,
  results: Object
})

onMounted(() => {
  // console.log(props.results.userPoints)
})
</script>
