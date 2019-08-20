<template>
    <ul class="list-unstyled">
        <li v-for="sport in sortedSports">
            <a v-bind:href="'/sports/' + sport.sportId + '/edit'" class="btn btn-link">{{sport.name}}</a>
        </li>
    </ul>
</template>

<script>
    export default {
        name: "sport-sidebar",
        data() {
            return {
                // A computed property can't be used
                // because `data` is evaluated first.
                sports: [],
            }
        },
        computed: {
            sortedSports() {
                return this.sports.sort(function (a, b) {
                    var aUpper = a.name.toUpperCase();
                    var bUpper = b.name.toUpperCase();

                    if (aUpper < bUpper) {
                        return -1;
                    }
                    if (aUpper > bUpper) {
                        return 1;
                    }
                    return 0;
                });
            }
        },
        created() {
            var self = this;

            $.get('/siteapi/sports', function (data) {
                self.sports = data;
            }, 'json');
        },
    };
</script>