// Компонент продукта
Vue.component('product-basket', {
    props: ['data', 'index'],
    template: `
                <div v-if="productBasket != null" class="basket-block__table-item row m-0">
                    <div class="col-xl basket-block__table-item__image align-self-center">
                        <img v-if="productBasket.poster == null" src="/Images/food.png" alt="poster">
                        <img v-else v-bind:src="getImage(productBasket.poster)" alt="test">
                    </div>
                    <div class="col-xl basket-block__table-item__name-product align-self-center">
                        <h2>{{productBasket.name}}</h2>
                    </div>
                    <div class="col-xl basket-block__table-item__count-product align-self-center">
                        <div class="container">
                            <div class="row">
                                <div class="col-4 p-0"><input class="btn btn-primary w-100" v-on:click="decrement" type="button" value="-"></div>
                                <div class="col-4 p-0 text-center d-flex align-items-center justify-content-center"><span>{{productBasket.count}}</span></div>
                                <div class="col-4 p-0"><input class="btn btn-primary w-100" v-on:click="increment" type="button" value="+"></div>
                            </div>
                        </div>
                    </div>
                    <div class="mt-3 mt-xl-0 mb-3 mb-xl-0 col-xl basket-block__table-item__all-count-product align-self-center">
                        <span>{{productBasket.description}}</span>
                    </div>
                    <div class="col-xl basket-block__table-item__price-product align-self-center">
                        <div v-if="productBasket.procentDiscount != null">
                            <div class="basket-block__table-item__price-product__old-price text-right">
                                <span>{{productBasket.price * productBasket.count}}</span> €
                            </div>
                            <div class="basket-block__table-item__price-product__new-price">
                                <span style="font-weight: bold">{{((productBasket.price - (productBasket.price / 100 * productBasket.procentDiscount)) * productBasket.count).toFixed(2)}} €</span> 
                            </div>
                        </div>
                        <div v-else>
                            <div class="basket-block__table-item__price-product__new-price">
                                <span style="font-weight: bold">{{productBasket.price * productBasket.count}}</span> €
                            </div>
                        </div>
                    </div>
                    <div class="col-xl basket-block_table-item__remove-product align-self-center">
                        <button v-on:click="clickHandler" class="btn p-0" type="button"><img src="/Images/removeButton.png" alt="test"></button>
                    </div>
                </div>
            `,
    data: function () {
        return {
            productBasket: this.data,
            indexBasket: this.index
        }
    },
    mounted() {

    },
    methods: {
        updateCard: function () {

            var data = {
                id: this.productBasket.id,
                count: this.productBasket.count
            }

            axios.post(('updateCountProduct'), data)
                .then(
                    response => {

                    });
        },
        removeItem: function () {
            var data = {
                id: this.productBasket.id
            }

            axios.post(('RemoveItemBasket'), data)
                .then(
                    response => {

                    });
        },
        decrement: function () {
            if (this.productBasket.count > 1) {
                this.productBasket.count--;

                // Обновить кеш
                this.updateCard();
            }
        },
        increment: function () {
            if (this.productBasket.count < this.productBasket.amount) {
                this.productBasket.count++;

                // Обновить кеш
                this.updateCard();
            }
        },
        clickHandler: function () {
            this.removeItem();
            this.$emit('custom', this.productBasket.id);

            
        },
        getImage: function (array) {
            return 'data:image/jpg;base64, ' + array
        }
    }
})
