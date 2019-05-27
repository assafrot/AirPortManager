<script>
	import AirplaneList from "./components/AirplaneList.svelte";
	import NodeCanvas from "./components/NodeCanvas.svelte";
	import List from "./components/List.svelte";
	import {AirportService} from "./services/AirportService"

	let airportService = new AirportService();

</script>

<style>
	h1 {
		color: white;
	}
	.main {
		text-align: center;
	}
</style>

<div class="main">
	<h1>Airport</h1>
	<NodeCanvas></NodeCanvas>
	<List></List>
</div>
