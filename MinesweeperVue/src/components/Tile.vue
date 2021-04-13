<template>
    <v-btn tile :elevation="getHeight()" @click="explore" @click.right="toggleFlag" :color="tileStates[tileValue].color" small>
        <v-icon v-if="tileStates[tileValue].icon">{{tileStates[tileValue].value}}</v-icon>
        <div v-if="!tileStates[tileValue].icon">{{tileStates[tileValue].value}}</div>
    </v-btn>
</template>

<script>

export default {
    props: {
        tileValue: {
            type: Number,
            default: 0
        },
        xValue: {
            type: Number,
            default: 0
        },
        yValue: {
            type: Number,
            default: 0
        }

    },
    data: function() {
        return {
            tileStates: [
                {state: "Hidden", icon: false, value:"", color:"", flat:false},
                {state: "Empty", icon: false, value:"", color:"blue-grey lighten-4", flat:true},
                {state: "One", icon: false, value:"1", color:"blue-grey lighten-4", flat:true},
                {state: "Two", icon: false, value:"2", color:"blue-grey lighten-4", flat:true},
                {state: "Three", icon: false, value:"3", color:"blue-grey lighten-4", flat:true},
                {state: "Four",icon: false, value:"4", color:"blue-grey lighten-4", flat:true},
                {state: "Five",icon: false, value:"5", color:"blue-grey lighten-4", flat:true},
                {state: "Six",icon: false, value:"6", color:"blue-grey lighten-4", flat:true},
                {state: "Seven",icon: false, value:"7", color:"blue-grey lighten-4", flat:true},
                {state: "Eight",icon: false, value:"8", color:"blue-grey lighten-4", flat:true},
                {state: "Explosion",icon: true, value:"mdi-nuke", color:"red darken-2", flat:true},
                {state: "Flagged",icon: true, value:"mdi-flag-variant", color:"green lighten-4", flat:false},
                {state: "DiscoveredMine",icon: true, value:"mdi-mine", color:"grey lighten-2", flat:true},
                {state: "MisplacedFlag",icon: true, value:"mdi-flag-variant", color:"red lighten-2", flat:true}
            ]
        }
    },
    methods: {
        getHeight() {
            return this.tileStates[this.tileValue].flat ? 0 : 1;
        },
        explore() {
            this.$emit('explored', this.xValue, this.yValue)
        },
        toggleFlag(event) {
            if (this.tileStates[this.tileValue].state === "Hidden") {
                this.$emit('flagged', this.xValue, this.yValue)
            }
            else if (this.tileStates[this.tileValue].state === "Flagged") {
                this.$emit('unflagged', this.xValue, this.yValue)
            }
            event.preventDefault()
        }
    }
}
</script>
<style>
</style>