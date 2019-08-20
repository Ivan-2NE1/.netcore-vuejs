import InputField from "../components/base/input-field/input-field.vue";

var teamInfo = new Vue({
    el: '#vueApp',

    components: {
        "input-field": InputField
    },

    data: {
        team: window.team,
    },

    created() {
        // Load whatever we need to get via ajax (not included in the Model)
        // Setup any non-reactive properties here
        var vm = this;

    },

    mounted() {
        // Anything that needs to happen after the DOM is ready
        var vm = this;
    },

    computed: {

    },

    methods: {

    }
});