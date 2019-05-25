<script>
	import AirplaneList from "./components/AirplaneList.svelte";
	import NodeCanvas from "./components/NodeCanvas.svelte";
	import {AirportService} from "./services/AirportService.ts"

	let airportService = new AirportService();

</script>

<style>
	h1 {
		color: purple;
	}
</style>

<h1>== Airport ==</h1>

<NodeCanvas ></NodeCanvas >
