<template>
    <form>
        <widget-wrapper v-bind:icon="'whistle'" v-bind:title="'Choose a Sport'"
                        v-bind:padding="true">

            <sport-list v-bind:default-value="sportId"
                        v-bind:enable-multiple="false"
                        v-bind:value.sync="selectedSport"
                        v-on:change="setSportId"></sport-list>
            
            <template v-slot:footer>
                <!--<button type="submit" class="btn btn-primary btn-lg" v-bind:disabled="!isValid">Continue</button>-->
                <router-link to="chooseTeam" tag="button" class="btn btn-primary btn-lg" v-bind:disabled="sportId === null || sportId <= 0">
                    Continue
                </router-link>
            </template>
        </widget-wrapper>
    </form>
</template>

<script>
    import WidgetWrapper from "../../components/base/widget-wrapper/widget-wrapper.vue";
    import SportList from "../../components/select-lists/sport-list.vue";

    export default {
        name: "choose-sport",

        components: {
            "widget-wrapper": WidgetWrapper,
            "sport-list": SportList,
        },

        computed: {
            sportId() {
                return this.$store.state.addGame.sportId;
            },

            isValid() {
                return this.sportId !== null && this.sportId > 0;
            },
        },

        data: function () {
            return {
                selectedSport: null,
            }
        },

        methods: {
            setSportId(newVal) {
                this.selectedSport = newVal;
                this.$store.commit("selectSport", newVal);
            }
        },

        mounted() {
            var vm = this;
            //document.getElementsByClassName("vs__search")[0].focus();
            document.addEventListener('keydown', function (event) {
                if (event.code == "Enter") {
                    var paths = window.location.href.split("/");
                    var lastPath = paths[paths.length - 1];
                    if (lastPath != "Report") {
                        return;
                    }
                    vm.$router.push({ name: 'chooseTeam', params: {} });
                }
            });
        }
    }

</script>