var app = new Vue({
    el: "#app",
    data: {
        newsid: (new URL(document.location.href)).searchParams.get("id"),
        blocks: [],
        isUploading: false,
        themeMax: 100,
        sotNameMax: 30,
        sotPosMax: 50,
        geoMax:30
    },
    methods: {
        upload: async function () {
            if (this.isUploading)
                retrun;
            this.isUploading = true;
            var elem = document.getElementById("tBtn")
            var txt = elem.innerHTML;
            elem.innerHTML = "выгружаю..."
            try {
                var dt = this.blocks.filter(function (b) { return b.titles.length > 0 });
                var ret = [];
                dt.forEach(d => {
                    d.titles.forEach(t => {
                        ret.push(t);
                    })
                })
                var ret = await axios.post("/testservice.asmx/titleToExcel", { titles: JSON.stringify(ret) });
                
                if (ret.data.d.Result!="401")
                    elem.innerHTML = "Выгружено";
                else
                    elem.innerHTML = "Ошибка: Вы не авторизованы";
                setTimeout(() => { elem.innerHTML = txt; this.isUploading = false; }, 4000)
               

            } catch (e) {
                console.warn(e)
                elem.innerHTML = "ошибка, повторите еще раз";
                setTimeout(() => { elem.innerHTML = txt; this.isUploading=false},4000)
         
            }
        }
    },
    mounted:async  function () {
        console.log('vue ready', this.newsid);
        var blocks = await axios.get("/API/BlocksJson/" + this.newsid);
        setTimeout(() => { 
        blocks = blocks.data.filter(b => b.ParentId==0)
        
            blocks.forEach(b => {
                console.log(b.BlockText)
                while (b.BlockText && b.BlockText.indexOf("NF::BOLDSTART") >= 0)
                    b.BlockText = b.BlockText.replace("NF::BOLDSTART", "").replace("NF::BOLDEND", "");
                this.blocks.push({
                    type: b.TypeName,
                    text: b.Text,
                    name: b.Name,
                    titles: getTitles(b.BlockText, b.Id, b.Name) 
                })
            })
        }, 1000)

    }
})
function getTitles(text, blockid, blockname) {
    if (text) {
        var ret=[]
        var matches = text.match(/\(\([^\)]+\)\)/gi)
        if (!matches)
            return [];
        matches.forEach(m => {
  
            let type = m.match(/^\(\(GEO:([^\)]+)\)/)
            if (type) {
                return ret.push({ type: "GEO", text: type[1].trim(), blockid, blockname })
            }
            type = m.match(/^\(\(SOURCE:([^\)]+)\)/)
            if (type) {
                return ret.push({ type: "SRC", text: type[1].trim(), blockid, blockname })
            }
            type = m.match(/^\(\(THEME:([^\)]+)\)/)
            if (type) {
                return ret.push({ type: "THM", text: type[1].trim(), blockid, blockname })
            }
            type = m.match(/^\(\(SOT([^\)]+)\)/)
            if (type){
                var name = m.match(/NAME:([^\n]+)/)
                var pos = m.match(/TITLE:([^\n]+)/)
                if (!name)
                    name = ["", ""]
                if (!pos)
                    pos = ["", ""]
               
                return ret.push({ type: "SOT", text: name[1].trim() + " " + pos[1].trim(), name: name[1].trim(), pos: pos[1].trim(), blockid, blockname})
            }

            //return ret.push({ type: "PLAIN", text: m.trim() })
        })
        return ret;

        matches.forEach()
    }
    else
        return [];

}