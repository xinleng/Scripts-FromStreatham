using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using MWM.JSON.Standard;


namespace MWM.JSON.Standard
{
	//PHASES
	[System.Serializable]
	public class MWMPhasesModel
	{
		public string error ;
		public List<MWMPhase> phases ;
	}
	
	[System.Serializable]
	
	public class MWMPhase
	{
		public int phase_id;
		public string phase_name;
	}
	
	//BLOCKS
	[System.Serializable]
	public class MWMBlocksModel 
	{
		public string error ;
		public List<MWMBlock> blocks ;
	}
	
	[System.Serializable]
	public class MWMBlock
	{
		public int block_id ;
		public int block_phase;
		public string block_name ;
	}
	
	//CORES
	[System.Serializable]
	public class MWMCoresModel
	{
		public string error ;
		public List<MWMCore> cores ;
	}
	
	[System.Serializable]
	public class MWMCore
	{
		public int core_id ;
		public int core_block;
		public string core_name ;
	}
	
	//FLOORS
	[System.Serializable]
	public class MWMFloorsModel
	{
		public string error ;
		public List<MWMFloor> floors ;
	}
	[System.Serializable]
	public class MWMFloor
	{
		public int floor_id ;
		public int floor_core;
		public string floor_name ;
	}
	
	//UNITS
	[System.Serializable]
	public class MWMUnitsModel
	{
		public string error ;
		public List<MWMUnit> units ;
	}
	
	[System.Serializable]
	public class MWMUnit
	{
		
		[Header ("ID")]
		//data types comes from CMS
		public int unit_id ;
		public string unit_name ;
		public int unit_type ;
		public string unit_aspect;
		public string unit_view;
		public int unit_floor ;
		public string unit_price ;
		public string unit_DMX_A ;
		public string unit_DMX_B ;
		public string unit_DMX_C ;
		public int unit_status ;
		public string unit_CGI;
		public string unit_plan;
		public string unit_beds;
		public string unit_storeys;
		public string unit_sqft;
		public string unit_sqm;
		
		[Header ("NAMES")]
		//to be generate at run time 
		public string unitBedroomName;
		public string unitFloorName;
		public string unitBlockName;
		public string unitCoreName;
		public string unitStatusName;
		public string[] unitViewsArray;
		
	}
	
	//ASPECT
	[System.Serializable]
	public class MWMAspectModel
	{
		public string error ;
		public List<MWMAspect> aspects ;
	}
	[System.Serializable]
	public class MWMAspect
	{
		public int aspect_id ;
		public string aspect_name ;
	}
	
	//BEDROOMS
	[System.Serializable]
	public class MWMBedroomsModel
	{
		public string error ;
		public List<MWMBedroom> bedrooms ;
	}
	[System.Serializable]
	public class MWMBedroom
	{
		public int bedroom_id ;
		public string bedroom_name ;
	}
	
	//DUPLEX
	[System.Serializable]
	public class MWMDuplexModel
	{
		public string error ;
		public List<MWMDuplex> duplex ;
	}
	
	[System.Serializable]
	public class MWMDuplex 
	{
		public int duplex_id ;
		public string duplex_name ;
	}
	
	//OUTSIDE
	[System.Serializable]
	public class MWMOutsideModel
	{
		public string error ;
		public List<MWMOutside> outside ;
	}
	[System.Serializable]
	public class MWMOutside
	{
		public int outside_id ;
		public string outside_name ;
	}
	
	//STATUS
	[System.Serializable]
	public class MWMStatusModel
	{
		public string error ;
		public List<MWMStatus> statuses ;
	}
	[System.Serializable]
	public class MWMStatus
	{
		public int status_id ;
		public string status_name ;
	}
	
	//TENURE
	[System.Serializable]
	public class MWMTenureModel
	{
		public string error ;
		public List<MWMTenure> tenure ;
	}
	[System.Serializable]
	public class MWMTenure 
	{
		public int tenure_id ;
		public string tenure_name ;
	}
	
	//TYPES
	[System.Serializable]
	public class MWMTypeModel
	{
		public string error ;
		public List<MWMType> types ;
	}
	
	[System.Serializable]
	public class MWMType
	{
		public int type_id ;
		public string type_name ;
		public string type_sqm ;
		public string type_sqft ;
		//public int type_bedrooms ;// bed room now moved to units.
		//public int type_wheelchair ;
		//public int type_duplex ;
		//public int type_aspect ;
		//public int type_outside_space ;
		//public string type_outside_sqm ;
		//public string type_outside_sqft ;
		
	}
	
	//WHEELCHAIR
	[System.Serializable]
	public class MWMWheelchairModel
	{
		public string error ;
		public List<MWMWheelchair> wheelchair ;
	}
	
	[System.Serializable]
	public class MWMWheelchair
	{
		public int wheelchair_id ;
		public string wheelchair_name ;
	}
	
	//PriceRange
	[System.Serializable]
	public class MWMPricerangeModel
	{
		public string error ;
		public List<MWMPricerange> priceranges ;
	}
	
	[System.Serializable]
	public class MWMPricerange
	{
		public MWMPricerange()
			
		{
			pricerange_id=0;
			pricerange_start=0;
			pricerange_end=0;
		}
		
		public int pricerange_id ;
		public int pricerange_start ;
		public int pricerange_end;
		
	}
	
	//Updates
	
	[System.Serializable]
	public class MWMUpdateModel
	{
		public string error ;
		public List<MWMUpdate> updates;
	}
	
	[System.Serializable]
	public class MWMUpdate
	{
		public int updates_id;
		public int updates_status;
	}
	
	
	
}
