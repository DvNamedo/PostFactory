using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;
using UnityEngine.Networking;

namespace Grid.Block
{
    public class BlockPlace : NetworkBehaviour
    {
        public TileTypeDatabase.TileEntry.placetype RequestingTileType; // 배치하려는 타일 타입

        // 참조를 에디터에서 연결하거나 Awake에서 자동 검색
        public TileReader tileReader;
        
        void Awake()
        {
            if (IsServer)
                return;
                if (tileReader == null)
                    tileReader = FindObjectOfType<TileReader>();
        }
        public void place(float X, float Y, NetworkObjectReference block)
        {
            if (tileReader == null || tileReader.tilemap == null)
                return;

            // 현재 월드 위치를 셀 좌표로 변환
            Vector3 worldPos = new Vector3(X, Y, 0f);
            Vector3Int cell = tileReader.tilemap.WorldToCell(worldPos);
            Vector2Int cell2 = new Vector2Int(cell.x, cell.y);

            // 타일 데이터 읽기
            TileBase tile = tileReader.tilemap.GetTile(cell);
            var placeType = tileReader.PlaceType(cell2);
            var entry = tileReader.database != null ? tileReader.database.GetEntryInternal(tile) : null;
            // 여기서 entry나 tileType을 이용해 배치 로직을 추가하세요.
            if(entry.placeType == RequestingTileType)
            {
                PlaceRequestServerRpc(X,Y,block);
            }
        }

        [ServerRpc]
        void PlaceRequestServerRpc(float x, float y, NetworkObjectReference block)
        {
            PlaceResponseClientRpc(x,y,block);
        }
        [ClientRpc]
        void PlaceResponseClientRpc(float x, float y, NetworkObjectReference block)
        {
            if (block.TryGet(out NetworkObject obj))
            {
                GameObject Block = obj.gameObject;
                Instantiate(Block, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
    }
}

//TODO 마그네틱 기능
/*Place(설치의 x좌표, 설치의 y좌표, 설치할 블록)
    -x, y는 설치하려는 장소의 좌표, block은 설치하려는 블록의 프리팹
    -타일맵에서 해당 좌표의 타일 정보를 읽어와서, 배치하려는 블록이 설치 가능한 타일인지 확인
    -설치 가능한 타일이라면, 서버에 설치 요청을 보내고, 서버는 모든 클라이언트에게 설치 응답을 보내서 블록을 생성하도록 함 */
