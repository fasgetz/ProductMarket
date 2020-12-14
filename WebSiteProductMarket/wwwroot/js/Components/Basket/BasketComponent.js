
var basket = Vue.component('basket', {
    template: `
            <div v-if="productBasket.products.length != 0" class="basket-block container-fluid">
                <div class="basket-block__header text-center">
                    <h1 class="basket-block__header-h1">Корзина товаров</h1>
                </div>
                <div class="basket-block__table text-center">
                    <div class="basket-block__table-header row m-0">
                        <div class="col-xl d-none d-xl-block basket-block__table-header__column">
                            Изображение
                        </div>
                        <div class="col-xl d-none d-xl-block basket-block__table-header__column">
                            Название
                        </div>
                        <div class="col-xl d-none d-xl-block basket-block__table-header__column">
                            Количество
                        </div>
                        <div class="col-xl d-none d-xl-block basket-block__table-header__column">
                            Наличие
                        </div>
                        <div class="col-xl d-none d-xl-block basket-block__table-header__column">
                            Стоимость
                        </div>
                        <div class="col-xl d-none d-xl-block basket-block__table-header__column">
                            Удалить
                        </div>
                    </div>
                    <div v-if="loadData == true">
                        <product-basket v-for="(item, index) in productBasket.products" :key="item.id" v-on:custom="removeHandler" :index="index" :data="item"></product-basket>
                    </div>
                    <div class="basket-block__table-result m-0">
                        <div class="basket-block__table-result__statistic text-right">
                            <div>
                                Всего товаров: {{watchCount}}
                            </div>
                            <div class="basket-block__table-result__statistic__discount">
                                Скидка: {{watchDiscount}} руб.
                            </div>
                            <div>
                                Итого: {{watchTotalPrice}} руб.
                            </div>
                        </div>
                        <div class="row">
                            <div class="col text-left">
                                <button v-on:click="GoIndex" class="btn btn-primary">Вернуться к покупкам</button>
                            </div>
                            <div class="col text-right">
                                <button v-on:click="GoPay" class="btn btn-warning">Оформить заказ</button>                                
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div v-else class="basket-block container-fluid">
                <div class="basket-block__header text-center">
                    <h1 class="basket-block__header-h1">Корзина товаров пуста</h1>
                    <div class="basket-block__go-back">
                        <button v-on:click="GoIndex" class="btn btn-primary">Вернуться к покупкам</button>
                    </div>
                </div>
            </div>
            `,
    data: () => ({
        productBasket: null,
        loadData: false,
        totalCount: 0,
        totalPrice: 0,
        totalDiscount: 0
    }),
    methods: {
/*        AuthUser: function () {
            $('#loginButton').click();
        },*/
        GoPay: function () {
            window.location.href = urlApp + "basket/pay"
        },
        GoIndex: function () {
            window.location.href = urlApp + "Home/Index"
        },
        GetCart: function () {
            // Загружаем из кеша, что добавлено в корзину
            return axios.get(('GetCartJson'))
                .then(
                    response => {
                        this.productBasket = response.data;
                        return true;
                    });
        },
        loadItems: function () {
            axios.post((urlApi + 'api/basket/getBasketProducts'), this.productBasket)
                .then(
                    response => {
                        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        this.productBasket = response.data;
                        // force update to make sure update hook fires
                        this.loadData = true;
                        /*this.authUser = true;*/
                    }).catch(error => {

                        // Если ошибка 401, то пользователь не авторизован
                        if (error.response.status == 401) {
                            /*this.authUser = false*/
                        }
                    });
        },
        removeHandler: function (id) {

            var pos = this.productBasket.products.map(function (e) { return e.id; }).indexOf(id);
            this.productBasket.products.splice(pos, 1);

        }
    },
    computed: {
        watchDiscount: function () {
            this.totalDiscount = 0;
            for (i = 0; i < this.productBasket.products.length; i++) {
                if (this.productBasket.products[i].procentDiscount != null) {
                    this.totalDiscount += ((this.productBasket.products[i].price / 100 * this.productBasket.products[i].procentDiscount)) * this.productBasket.products[i].count
                }
            }

            return this.totalDiscount;
        },
        watchCount: function () {
            this.totalCount = 0;
            for (i = 0; i < this.productBasket.products.length; i++) {
                this.totalCount += this.productBasket.products[i].count;
            }

            return this.totalCount;
        },
        watchTotalPrice: function () {
            this.totalPrice = 0;
            for (i = 0; i < this.productBasket.products.length; i++) {
                // Если у итема есть скидка, то посчитать с ней
                if (this.productBasket.products[i].DiscountProduct != null)
                    this.totalPrice += (this.productBasket.products[i].price - (this.productBasket.products[i].price / 100 * this.productBasket.products[i].DiscountProduct)) * this.productBasket.products[i].count
                else
                    this.totalPrice += this.productBasket.products[i].price * this.productBasket.products[i].count;
            }

            return this.totalPrice - this.totalDiscount;
        }
    },
    mounted() {

        axios.all([
            this.GetCart()
        ])
            .then(axios.spread((first_response) => {
                if (first_response == true) {
                    setTimeout(() => {
                        this.loadItems();
                    });
                }
            }))
    }
})
